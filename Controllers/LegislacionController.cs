using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using LegislacionAPP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SixLabors.ImageSharp;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

//[Authorize]
public class LegislacionController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private const string AliasAutogen = "Delitos_Venezuela.pdf";
    private string PlantillaPath =>
        Path.Combine(_env.WebRootPath, "plantillas", "ley_texto.html");

    public LegislacionController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }    
    private int UsuarioIdFallback() => 1; // para pruebas locales

    [HttpGet]
    public async Task<IActionResult> Index(int empresaId)
    {
        var userId = await GetUsuarioIdActualAsync() ?? UsuarioIdFallback();

        // Nombre de la empresa para el encabezado
        ViewBag.EmpresaId = empresaId;
        ViewBag.Empresa = await _context.Empresa
            .Where(e => e.id_empresa == empresaId)
            .Select(e => e.nombre)
            .FirstOrDefaultAsync();

        // Lista de legislaciones de esa empresa
        var modelo = await _context.Legislacion
            .Where(l => l.id_empresa == empresaId)
            .Include(l => l.id_estadoNavigation)
            .Select(l => new LegislacionRowVM
            {
                id_legislacion = l.id_legislacion,
                titulo = l.titulo,
                codigo_interno = l.codigo_interno,
                estado = l.id_estadoNavigation.nombre,
                color_hex = l.id_estadoNavigation.color_hex,
                fecha_creacion = l.fecha_creacion,
                fecha_vigencia = l.fecha_vigencia,
                certificable = l.Ciclos
                .Where(c => c.fecha_cierre != null)
                .OrderByDescending(c => c.fecha_cierre)
                .Select(c => (c.porcentaje_aprobado ??
                             (c.articulos_totales == 0 ? 0 : 100m * (decimal)c.articulos_aprobados / c.articulos_totales))
                             >= 100m)
                .FirstOrDefault()
            })
            .OrderByDescending(l => l.fecha_creacion)
            .ToListAsync();
        foreach(var item in modelo)
        {
            item.total_segmentos = await _context.SegmentoLegislacion
                .CountAsync(s => s.id_legislacion == item.id_legislacion);
        }
        return View(modelo);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var legislacion = await _context.Legislacion
            .Include(l => l.id_ambito_aplicacionNavigation)
            .Include(l => l.id_empresaNavigation)
            .Include(l => l.id_estadoNavigation)
            .FirstOrDefaultAsync(m => m.id_legislacion == id);
        if (legislacion == null)
        {
            return NotFound();
        }

        return View(legislacion);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int empresaId)
    {
        var empresa = await _context.Empresa.FindAsync(empresaId);
        if (empresa is null) return NotFound();

        var ambitos = await _context.AmbitoAplicacion
            .Select(a => new SelectListItem
            {
                Value = a.id_ambito_aplicacion.ToString(),
                Text = a.nombre
            })
            .ToListAsync();

        var vm = new LegislacionCreateVM
        {            
            id_empresa = empresaId,
            EmpresaNombre = empresa.nombre,
            Ambitos = ambitos
        };
        vm.Paises = new SelectList(_context.Pais, "id_pais", "nombre");

        return View(vm);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LegislacionCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Ambitos = await _context.AmbitoAplicacion
                .Select(a => new SelectListItem { Value = a.id_ambito_aplicacion.ToString(), Text = a.nombre })
                .ToListAsync();
            return View(vm);
        }

        var entidad = new Legislacion
        {
            id_empresa = vm.id_empresa,
            id_ambito_aplicacion = vm.id_ambito_aplicacion,
            titulo = vm.titulo,
            subtitulo = vm.subtitulo,
            alias = vm.alias,
            codigo_interno = vm.codigo_interno,
            fecha_vigencia = vm.fecha_vigencia,
            fecha_creacion = DateTime.UtcNow,
            id_usuario = await GetUsuarioIdActualAsync() ?? UsuarioIdFallback(),
            id_pais = vm.id_pais,
            id_estado = (int)EstadoCodigo.Activa  
        };

        _context.Add(entidad);
        await _context.SaveChangesAsync();
        bool autoCreate = false;
        if (vm.Pdf != null && vm.Pdf.Length > 0)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "legislaciones", entidad.id_legislacion.ToString());
            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(vm.Pdf.FileName);
            var fileName = $"documento{ext}";
            var dest = Path.Combine(folder, fileName);

            using (var fs = new FileStream(dest, FileMode.Create))
                await vm.Pdf.CopyToAsync(fs);

            entidad.pdf_url = $"/uploads/legislaciones/{entidad.id_legislacion}/{fileName}";
            _context.Update(entidad);
            autoCreate = vm.Pdf.FileName.ToLower() == AliasAutogen.ToLower() ? true : false ;
            await _context.SaveChangesAsync();
        }

        if (autoCreate)
        {
            var raw = await System.IO.File.ReadAllTextAsync(PlantillaPath);
            var parsed = ParseSegments(raw); // | y %

            int orden = 1;
            foreach (var seg in parsed)
            {
                _context.SegmentoLegislacion.Add(new SegmentoLegislacion
                {
                    id_legislacion = entidad.id_legislacion,
                    tituloSegmento = seg.Nombre,
                    contenido = seg.Html,
                    id_tipo_elemento = (int)seg.Tipo,
                    orden = orden++,
                    fecha_creacion = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();

        }

        TempData["msg"] = "Legislación creada correctamente.";
        return RedirectToAction("Edit", new { id = entidad.id_legislacion });
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var legislacion = await _context.Legislacion
            .Include(l => l.id_ambito_aplicacionNavigation)
            .Include(l => l.id_empresaNavigation)
            .Include(l => l.id_estadoNavigation)
            .FirstOrDefaultAsync(m => m.id_legislacion == id);
        if (legislacion == null)
        {
            return NotFound();
        }

        return View(legislacion);
    }

    // POST: LegislacionTEST/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var legislacion = await _context.Legislacion.FindAsync(id);
        if (legislacion != null)
        {
            _context.Legislacion.Remove(legislacion);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Legislacion/Edit?id=5&empresaId=2  (con segmentos)
    [HttpGet]
    public async Task<IActionResult> Edit(int id, int? empresaId = null)
    {
        var l = await _context.Legislacion
        .Include(x => x.Segmentos)
        .FirstOrDefaultAsync(x => x.id_legislacion == id);
        if (l is null) return NotFound();

        var fotoId = await _context.TipoElemento
            .Where(t => t.nombre == "Imagen")
            .Select(t => t.id_tipo_elemento)
            .FirstOrDefaultAsync();

        var vm = new LegislacionEditVM
        {
            id_legislacion = l.id_legislacion,
            id_empresa = l.id_empresa,
            id_estado = l.id_estado,
            id_ambito_aplicacion = l.id_ambito_aplicacion,
            id_pais = l.id_pais,
            titulo = l.titulo,
            subtitulo = l.subtitulo,
            alias = l.alias,
            codigo_interno = l.codigo_interno,
            fecha_vigencia = l.fecha_vigencia,
            pdf_url = l.pdf_url,
            IdTipoElementoFoto = fotoId,
            Segmentos = l.Segmentos            
                .OrderBy(s => s.orden)
                .Select(s => new SegmentoVM
                {
                    id_segmento_legislacion = s.id_segmento_legislacion,
                    id_legislacion = s.id_legislacion,
                    id_tipo_elemento = s.id_tipo_elemento,
                    contenido = s.contenido,
                    observaciones = s.observaciones,
                    orden = s.orden,
                    archivo_url = s.contenido_url,
                    segmentoTitulo = s.tituloSegmento
                }).ToList(),
            Paises = new SelectList(_context.Pais, "id_pais", "nombre", l.id_pais),
            Empresas = new SelectList(_context.Empresa, "id_empresa", "nombre", l.id_empresa),
            Estados = new SelectList(_context.Estado, "id_estado", "codigo", l.id_estado),
            Ambitos = new SelectList(_context.AmbitoAplicacion, "id_ambito_aplicacion", "nombre", l.id_ambito_aplicacion),
            TiposElemento = new SelectList(_context.TipoElemento, "id_tipo_elemento", "nombre")
        };

        return View(vm);
    }

    // POST: Legislacion/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(LegislacionEditVM vm)
    {
        // 1) Actualizar cabecera
        var l = await _context.Legislacion
            .Include(x => x.Segmentos)
            .FirstOrDefaultAsync(x => x.id_legislacion == vm.id_legislacion);
        if (l is null) return NotFound();

        l.id_empresa = vm.id_empresa;
        l.id_estado = vm.id_estado;
        l.id_ambito_aplicacion = vm.id_ambito_aplicacion;
        l.titulo = vm.titulo;
        l.subtitulo = vm.subtitulo;
        l.alias = vm.alias;
        l.codigo_interno = vm.codigo_interno;
        l.fecha_creacion = vm.fecha_creacion;
        l.fecha_actualizacion = DateTime.UtcNow;
        l.id_pais = vm.id_pais;

        // 2) Sincronizar hijos (upsert + delete)
        var existentes = l.Segmentos.ToDictionary(s => s.id_segmento_legislacion);
        var idsPost = new HashSet<int>();
        if (vm.Pdf is not null && vm.Pdf.Length > 0)
        {
            l.pdf_url = await GuardarArchivoAsync(vm.Pdf, $"uploads/legislacion/{l.id_legislacion}");
        }
        var nextOrder = l.Segmentos.Any() ? l.Segmentos.Max(s => s.orden) + 1 : 1;

        foreach (var s in vm.Segmentos)
        {
            s.id_legislacion = l.id_legislacion; // asegurar FK

            // eliminar marcados
            if (s.Eliminar && s.id_segmento_legislacion.HasValue)
            {
                var segDel = existentes[s.id_segmento_legislacion.Value];
                _context.SegmentoLegislacion.Remove(segDel);
                await _context.SaveChangesAsync();
                continue;
            }

            if (s.id_segmento_legislacion.HasValue && existentes.ContainsKey(s.id_segmento_legislacion.Value))
            {
                if (s.id_tipo_elemento == 7)
                {
                    if (s.archivo is not null && s.archivo.Length > 0)
                    {
                        var img = await GuardarImagenAsync(
                            s.archivo,
                            $"uploads/legislacion/{l.id_legislacion}/segmentos"
                        );

                        s.archivo_url = img.url;                        
                    }
                }
                // UPDATE
                var seg = existentes[s.id_segmento_legislacion.Value];
                seg.id_tipo_elemento = s.id_tipo_elemento;
                seg.contenido = s.contenido;
                seg.tituloSegmento = s.segmentoTitulo;
                seg.observaciones = s.observaciones;
                seg.orden = s.orden;                
                seg.contenido_url = l.pdf_url;             

                idsPost.Add(seg.id_segmento_legislacion);
            }
            else
            {
                var fotoURL= string.Empty;
                if (s.id_tipo_elemento == 7)
                {
                    if (s.archivo is not null && s.archivo.Length > 0)
                    {
                        var img = await GuardarImagenAsync(
                            s.archivo,
                            $"uploads/legislacion/{l.id_legislacion}/segmentos"  // carpeta relativa
                        );

                        fotoURL = img.url;
                    }
                }
                // INSERT
                var nuevo = new SegmentoLegislacion
                {

                    id_legislacion = l.id_legislacion,
                    id_tipo_elemento = s.id_tipo_elemento,
                    contenido = s.contenido,
                    observaciones = s.observaciones,
                    orden = nextOrder++,        // <-- AQUÍ
                    fecha_creacion = DateTime.UtcNow,
                    contenido_url = fotoURL,
                    tituloSegmento = s.segmentoTitulo

                };
                _context.SegmentoLegislacion.Add(nuevo);
            }
        }

       //Re ordenar variable orden
        var segs = await _context.SegmentoLegislacion
        .Where(x => x.id_legislacion == l.id_legislacion)
        .OrderBy(x => x.orden)
        .ThenBy(x => x.id_segmento_legislacion)
        .ToListAsync();

        int orden = 1;
        foreach (var seg in segs)
            seg.orden = orden++;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { empresaId = l.id_empresa });
    }
    // Renderizar una card vacía (AJAX)
    [HttpPost]
    public async Task<IActionResult> AddSegmentCard(int index)
    {
        var vm = new SegmentoVM { orden = index + 1 };

        ViewData["Prefix"] = $"Segmentos[{index}]";
        ViewData["Index"] = index;  // <- NUEVO
        ViewData["FotoId"] = 7;
        ViewData["TiposElemento"] = new SelectList(await _context.TipoElemento.ToListAsync(),
                                                   "id_tipo_elemento", "nombre");

        return PartialView("_SegmentCard", vm);
    }


    private async Task CargarCombosAsync(LegislacionEditVM vm)
    {
        vm.Empresas = new SelectList(await _context.Empresa.ToListAsync(), "id_empresa", "nombre", vm.id_empresa);
        vm.Estados = new SelectList(await _context.Estado.ToListAsync(), "id_estado", "nombre", vm.id_estado);
        vm.Ambitos = new SelectList(await _context.AmbitoAplicacion.ToListAsync(), "id_ambito_aplicacion", "nombre", vm.id_ambito_aplicacion);
        vm.TiposElemento = new SelectList(await _context.TipoElemento.ToListAsync(), "id_tipo_elemento", "nombre");
    }

    private async Task<string> GuardarArchivoAsync(IFormFile file, string carpetaRelativa)
    {
        var root = Path.Combine(_env.WebRootPath, carpetaRelativa.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(root);
        var nombre = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
        var full = Path.Combine(root, nombre);
        await using var fs = System.IO.File.Create(full);
        await file.CopyToAsync(fs);
        return $"/{carpetaRelativa}/{nombre}".Replace("//", "/");
    }
    private async Task<(string url, string mime, long bytes, int? width, int? height, string sha256)>
    GuardarImagenAsync(IFormFile file, string carpetaRelativa)
    {
        if (file is null || file.Length == 0)
            throw new InvalidOperationException("Archivo vacío.");

        // Acepta solo imágenes
        if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo se permiten imágenes.");

        var rootAbs = Path.Combine(_env.WebRootPath,
                        carpetaRelativa.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(rootAbs);

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var nombre = $"{Guid.NewGuid():N}{ext}";
        var absPath = Path.Combine(rootAbs, nombre);

        // Guardar en disco
        using (var fs = System.IO.File.Create(absPath))
            await file.CopyToAsync(fs);

        // URL relativa (para <img src> / <embed>)
        var url = "/" + Path.Combine(carpetaRelativa, nombre)
                        .Replace("\\", "/");

        // Leer metadatos básicos
        int? w = null, h = null;
        try
        {
            using var img = await Image.LoadAsync(absPath);
            w = img.Width;
            h = img.Height;
        }
        catch {}

        // SHA-256 para huella
        string hash;
        using (var sha = SHA256.Create())
        using (var st = System.IO.File.OpenRead(absPath))
            hash = Convert.ToHexString(sha.ComputeHash(st));

        return (url, file.ContentType, file.Length, w, h, hash);
    }
    private bool LegislacionExists(int id)
    {
        return _context.Legislacion.Any(e => e.id_legislacion == id);
    }

    [Authorize(Roles = "Digitalizador,Auditor,Administrador,Gerente")]
    public async Task<IActionResult> Ciclos(int legislacionId)
    {
        var l = await _context.Legislacion
            .Include(x => x.id_empresaNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id_legislacion == legislacionId);

        if (l == null) return NotFound();

        // Traer los ciclos de esta legislación (ordenados del más nuevo)
        var ciclos = await _context.CicloAuditoria
            .Where(c => c.id_legislacion == legislacionId && c.fecha_cierre.HasValue)
            .OrderByDescending(c => c.fecha_inicio)
            .Select(c => new
            {
                c.id_ciclo_auditoria,
                c.fecha_inicio,
                c.fecha_cierre,
                c.articulos_totales,
                c.articulos_aprobados,
                c.porcentaje_aprobado,
                c.id_usuario,
                c.id_estado
            })
            .ToListAsync();

        // resolver nombre del auditor
        var usuarioIds = ciclos.Where(x => x.id_usuario != 0).Select(x => x.id_usuario).Distinct().ToList();
        var nombres = await _context.Usuario
            .Where(u => usuarioIds.Contains(u.id_usuario))
            .ToDictionaryAsync(u => u.id_usuario, u => (u.nombre + " " + u.apellido).Trim());

        var vm = new CiclosPorLegislacionVM
        {
            id_legislacion = legislacionId,
            id_empresa = l.id_empresa,
            Empresa = l.id_empresaNavigation?.nombre ?? "",
            Legislacion = l.titulo,
            Ciclos = ciclos.Select(c => new CicloAuditoriaRowVM
            {
                id_ciclo_auditoria = c.id_ciclo_auditoria,
                fecha_inicio = c.fecha_inicio,
                fecha_cierre = c.fecha_cierre,                
                Auditor = (c.id_usuario != 0 && nombres.TryGetValue(c.id_usuario, out var nom)) ? nom : "(no asignado)",
                TotalSegmentos = c.articulos_totales,
                Aprobados = c.articulos_aprobados,
                Porcentaje = c.articulos_totales > 0
                    ? Math.Round((decimal)c.articulos_aprobados * 100m / c.articulos_totales, 2)
                    : (c.porcentaje_aprobado ?? 0m) // o usa el que tengas
                    
            }).ToList()
        };


        return View(vm);
    }

    private static readonly Regex ClassRegex =
        new Regex(@"<div\s+[^>]*class\s*=\s*""(?<cls>[^""]+)""", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private enum SegmentoTipo { Titulo = 1, Articulo = 4, Otro = 10 }

    private static SegmentoTipo MapTipo(string? classAttr)
    {
        if (string.IsNullOrWhiteSpace(classAttr)) return SegmentoTipo.Otro;
        var first = classAttr.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                             .FirstOrDefault()?.ToLowerInvariant();
        return first switch
        {
            "titulo" => SegmentoTipo.Titulo,
            "article" => SegmentoTipo.Articulo,
            _ => SegmentoTipo.Otro
        };
    }

    private sealed class ParsedSegment
    {
        public string Nombre { get; set; } = "";
        public string Html { get; set; } = "";
        public SegmentoTipo Tipo { get; set; }
    }

    private static List<ParsedSegment> ParseSegments(string raw)
    {
        var result = new List<ParsedSegment>();
        if (string.IsNullOrWhiteSpace(raw)) return result;

        // 1) Split por '|'
        var items = raw.Split('|', StringSplitOptions.RemoveEmptyEntries);

        foreach (var itemRaw in items)
        {
            var item = itemRaw.Trim();
            if (item.Length == 0) continue;

            // 2) "Nombre%HTML"
            var idx = item.IndexOf('%');
            if (idx <= 0 || idx >= item.Length - 1) continue;

            var nombre = item[..idx].Trim();
            var html = item[(idx + 1)..].Trim();
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(html)) continue;

            // 3) Detectar class del primer <div>
            var m = ClassRegex.Match(html);
            var classAttr = m.Success ? m.Groups["cls"].Value : null;

            result.Add(new ParsedSegment
            {
                Nombre = nombre,
                Html = html,
                Tipo = MapTipo(classAttr)
            });
        }
        return result;
    }
    private async Task<int?> GetUsuarioIdActualAsync()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(email)) return null;
        return await _context.Usuario
            .Where(u => u.correo == email)
            .Select(u => (int?)u.id_usuario)
            .FirstOrDefaultAsync();
    }

}

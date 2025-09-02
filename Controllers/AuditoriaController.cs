using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using LegislacionAPP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

[Authorize(Roles = "Auditor,Admin")]
public class AuditoriaController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AuditoriaController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    // 1) LISTA de ciclos por legislación
    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        var l = await _context.Legislacion
            .Include(x => x.id_empresaNavigation)
            .FirstOrDefaultAsync(x => x.id_legislacion == id);

        if (l is null) return NotFound();

        var ciclos = await _context.CicloAuditoria
            .Where(c => c.id_legislacion == id)
            .OrderByDescending(c => c.fecha_inicio)
            .Select(c => new CicloResumenVM
            {
                id_ciclo_auditoria = c.id_ciclo_auditoria,
                fecha_inicio = c.fecha_inicio,
                fecha_cierre = c.fecha_cierre,
                articulos_totales = c.articulos_totales,
                articulos_aprobados = c.articulos_aprobados,
                porcentaje_avance = c.porcentaje_aprobado ?? 0m
            })
            .ToListAsync();

        var vm = new AuditoriaCiclosVM
        {
            id_legislacion = id,
            EmpresaNombre = l.id_empresaNavigation?.nombre,
            Titulo = l.titulo,
            Ciclos = ciclos,
            EmpresaID = l.id_empresaNavigation?.id_empresa
        };

        return View(vm);
    }

    // CREAR nuevo ciclo (si no hay abierto)
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(int legislacionId)
    {
        var hayAbierto = await _context.CicloAuditoria
            .AnyAsync(c => c.id_legislacion == legislacionId && c.fecha_cierre == null);

        if (hayAbierto)
        {
            TempData["err"] = "Ya existe una auditoría abierta. Cierra la actual antes de crear una nueva.";
            return RedirectToAction(nameof(Index), new { id = legislacionId });
        }

        var totalSeg = await _context.SegmentoLegislacion
            .CountAsync(s => s.id_legislacion == legislacionId);

        var ciclo = new CicloAuditoria
        {
            id_legislacion = legislacionId,
            fecha_inicio = DateTime.UtcNow,
            articulos_totales = totalSeg,
            articulos_aprobados = 0,
            porcentaje_aprobado = 0m,
            id_usuario = (int)TryGetUserId(),
            id_estado = (int)EstadoCodigo.Activa

        };

        _context.CicloAuditoria.Add(ciclo);
        await _context.SaveChangesAsync();

        TempData["msg"] = "Auditoría creada.";
        return RedirectToAction("Index", new { id = ciclo.id_legislacion });
    }

    [HttpGet]
    [Authorize(Roles = "Auditor,Admin")]
    public async Task<IActionResult> Continuar(int cicloId)
    {
        var ciclo = await _context.CicloAuditoria
            .Include(c => c.id_legislacionNavigation)
                .ThenInclude(l => l.id_empresaNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.id_ciclo_auditoria == cicloId);

        if (ciclo is null) return NotFound();

        var l = ciclo.id_legislacionNavigation;

        // 1) Diccionario de tipos (id -> nombre)
        var tipos = await _context.TipoElemento
            .AsNoTracking()
            .ToDictionaryAsync(t => t.id_tipo_elemento, t => t.nombre);

        // 2) Evaluaciones del ciclo (lista y luego diccionario por id_segmento)
        var evalList = await _context.EvaluacionSegmento
            .Where(e => e.id_ciclo_auditoria == cicloId)
            .AsNoTracking()
            .ToListAsync();

        var evalPorSegmento = evalList.ToDictionary(e => e.id_segmento_legislacion);

        // 3) Evidencias agrupadas por id_evaluacion_segmento (para traerlas de una sola vez)
        var evalIds = evalList.Select(e => e.id_evaluacion_segmento).ToList();
        var evidenciasPorEval = await _context.Evidencia
            .Where(ev => evalIds.Contains(ev.id_evaluacion_segmento))
            .AsNoTracking()
            .GroupBy(ev => ev.id_evaluacion_segmento)
            .ToDictionaryAsync(g => g.Key, g => g.ToList());

        // 4) Segmentos vigentes de la legislación (ordenados para mostrar)
        var segmentos = await _context.SegmentoLegislacion
            .Where(s => s.id_legislacion == l.id_legislacion)
            .AsNoTracking()
            .OrderBy(s => s.orden)
            .ToListAsync();

        // (Opcional) nombre del auditor
        string? auditorNombre = null;
        if (ciclo.id_usuario != 0)
        {
            auditorNombre = await _context.Usuario
                .Where(u => u.id_usuario == ciclo.id_usuario)
                .Select(u => (u.nombre + " " + u.apellido).Trim())
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        var vm = new AuditoriaContinuarVM
        {
            id_legislacion = l.id_legislacion,
            id_ciclo_auditoria = ciclo.id_ciclo_auditoria,
            EmpresaNombre = l.id_empresaNavigation?.nombre,
            Titulo = l.titulo,
            AuditorNombre = auditorNombre,
            fecha_inicio = ciclo.fecha_inicio,
            fecha_cierre = ciclo.fecha_cierre,
            articulos_totales = ciclo.articulos_totales,
            articulos_aprobados = ciclo.articulos_aprobados,
            porcentaje_avance = (decimal)ciclo.porcentaje_aprobado,

            Segmentos = segmentos.Select(s =>
            {
                // Intentar obtener la evaluación del segmento (si existe)
                evalPorSegmento.TryGetValue(s.id_segmento_legislacion, out var ev);

                // Nombre del tipo de elemento
                tipos.TryGetValue(s.id_tipo_elemento, out var tipoNombre);

                // Evidencias de esa evaluación (si ya existe)
                List<Evidencia> evids = new();
                if (ev != null && evidenciasPorEval.TryGetValue(ev.id_evaluacion_segmento, out var lista))
                    evids = lista;

                // Mapea a tu VM de segmento
                return new SegmentoAuditableVM
                {
                    id_segmento_legislacion = s.id_segmento_legislacion,
                    id_evaluacion_segmento = ev?.id_evaluacion_segmento,       // null si aún no existe
                    contenido = s.contenido,
                    id_estado = ev?.id_estado,
                    observaciones = ev?.comentario,
                    id_tipo_elemento = s.id_tipo_elemento,
                    tipo_elemento = tipoNombre,
                    segmentoTitulo = s.tituloSegmento,
                    Evidencias = evids    // <<--- aquí van las evidencias precargadas
                };
            }).ToList()
        };

        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cerrar(int cicloId)
    {
        var ciclo = await _context.CicloAuditoria
            .FirstOrDefaultAsync(c => c.id_ciclo_auditoria == cicloId);
        if (ciclo is null) return NotFound();

        if (ciclo.fecha_cierre != null)
        {
            TempData["err"] = "El ciclo ya está cerrado.";
            return RedirectToAction(nameof(Index), new { id = ciclo.id_legislacion });
        }

        // Recalcula snapshot al cierre
        var total = ciclo.articulos_totales > 0
            ? ciclo.articulos_totales
            : await _context.SegmentoLegislacion.CountAsync(s => s.id_legislacion == ciclo.id_legislacion);


        ciclo.articulos_totales = total;
        ciclo.fecha_cierre = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        TempData["msg"] = "Auditoría cerrada.";
        return RedirectToAction(nameof(Index), new { id = ciclo.id_legislacion });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Guardar(AuditoriaContinuarVM vm, string accion, bool randomSave)
    {
        // accion: "guardar" | "cerrar" pare reconocer cuando cerrar el ciclo
        // 1) Validaciones básicas
        var ciclo = await _context.CicloAuditoria
            .FirstOrDefaultAsync(c => c.id_ciclo_auditoria == vm.id_ciclo_auditoria);

        if (ciclo == null || ciclo.id_legislacion != vm.id_legislacion)
            return NotFound();

        if (ciclo.fecha_cierre != null)
            return BadRequest("Este ciclo ya está cerrado.");

        var now = DateTime.UtcNow;

        // 2) Traer evaluaciones existentes del ciclo para los segmentos posteados
        var segIdsPost = vm.Segmentos.Select(s => s.id_segmento_legislacion).ToList();

        var evalsExistentes = await _context.EvaluacionSegmento
            .Where(e => e.id_ciclo_auditoria == ciclo.id_ciclo_auditoria &&
                        segIdsPost.Contains(e.id_segmento_legislacion))
            .ToDictionaryAsync(e => e.id_segmento_legislacion);


        // 3) Upsert de cada segmento
        foreach (var s in vm.Segmentos)
        {


            if (evalsExistentes.TryGetValue(s.id_segmento_legislacion, out var ev))
            {
                // UPDATE
                ev.id_estado = (int)s.id_estado == 0 && randomSave == true ? GetRandomInt() : (int)s.id_estado;
                ev.comentario = String.IsNullOrEmpty(s.observaciones) && randomSave ? GetRandomString(ev.id_estado) : s.observaciones;
                ev.fecha_actualizacion = now;
                ev.id_auditor = (int)TryGetUserId();
            }
            else
            {
                // INSERT
                int idestado = (int)s.id_estado == 0 && randomSave == true ? GetRandomInt() : (int)s.id_estado;
                var nuevo = new EvaluacionSegmento
                {
                    id_ciclo_auditoria = ciclo.id_ciclo_auditoria,
                    id_segmento_legislacion = s.id_segmento_legislacion,
                    id_estado = idestado,
                    comentario = String.IsNullOrEmpty(s.observaciones) && randomSave ? GetRandomString(idestado) : s.observaciones,
                    fecha_actualizacion = now,
                    id_auditor = (int)TryGetUserId()
                };
                _context.EvaluacionSegmento.Add(nuevo);
            }
        }

        await _context.SaveChangesAsync();

        // 4) Recalcular progreso del ciclo
        var total = await _context.SegmentoLegislacion
            .CountAsync(x => x.id_legislacion == ciclo.id_legislacion);

        var aprobados = await _context.EvaluacionSegmento
            .Where(e => e.id_ciclo_auditoria == ciclo.id_ciclo_auditoria && e.id_estado == (int)EstadoCodigo.Cumple)
            .CountAsync();

        var revisados = await _context.EvaluacionSegmento
            .Where(e => e.id_ciclo_auditoria == ciclo.id_ciclo_auditoria && e.id_estado != (int)EstadoCodigo.Cumple)
            .CountAsync();

        ciclo.articulos_totales = total;
        ciclo.articulos_aprobados = aprobados;
        ciclo.porcentaje_aprobado = total == 0 ? 0 : (int)Math.Round(100.0 * aprobados / total);
        ciclo.fecha_cierre = accion == "cerrar" ? DateTime.UtcNow : null;

        await _context.SaveChangesAsync();

        TempData["Ok"] = (accion == "cerrar")
            ? "Auditoría cerrada."
            : "Cambios guardados.";
        return RedirectToAction(nameof(Index), new { id = ciclo.id_legislacion });

    }

    // POST: /Auditoria/SubirEvidencia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubirEvidencia(int cicloId, int segId, IFormFile archivo)
    {
        if (archivo is null || archivo.Length == 0)
            return BadRequest("Archivo vacío.");

        // Solo imágenes o PDF
        var permitido = new[] { "application/pdf" };
        var esImagen = archivo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        if (!esImagen && !permitido.Contains(archivo.ContentType, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Formato no permitido.");

        // Carpeta: wwwroot/uploads/evidencias/ciclo_{id}/seg_{id}/
        var relDir = Path.Combine("uploads", "evidencias", $"ciclo_{cicloId}", $"seg_{segId}");
        var absDir = Path.Combine(_env.WebRootPath, relDir);
        Directory.CreateDirectory(absDir);

        // Nombre seguro + evitar colisiones
        var seguro = Path.GetFileNameWithoutExtension(archivo.FileName);
        var ext = Path.GetExtension(archivo.FileName);
        var stamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff");
        var saveName = $"{seguro}_{stamp}{ext}";
        var absPath = Path.Combine(absDir, saveName);

        await using (var fs = new FileStream(absPath, FileMode.Create))
            await archivo.CopyToAsync(fs);

        // URL pública
        var relUrl = "/" + Path.Combine(relDir, saveName).Replace('\\', '/');

        // Mime confiable por si el browser no lo manda
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(saveName, out var mime))
            mime = archivo.ContentType;

        var evidencia = new Evidencia
        {
            id_evaluacion_segmento = segId,
            nombre_archivo = Path.GetFileName(archivo.FileName),
            mime_type = mime,
            contenido_url = relUrl,
            fecha_subida = DateTime.UtcNow,
            id_usuario = (int)TryGetUserId()
        };

        _context.Evidencia.Add(evidencia);
        await _context.SaveChangesAsync();

        // Devolver la lista actualizada como parcial/HTML
        var items = await _context.Evidencia
            .Where(e => e.id_evaluacion_segmento == segId)
            .OrderByDescending(e => e.fecha_subida)
            .Select(e => new Evidencia
            {
                id_evidencia = e.id_evidencia,
                nombre_archivo = e.nombre_archivo,
                mime_type = e.mime_type,
                contenido_url = e.contenido_url,
                fecha_subida = e.fecha_subida
            }).ToListAsync();

        return PartialView("_EvidenciasList", items);
    }

    // POST: /Auditoria/EliminarEvidencia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarEvidencia(int id)
    {
        var ev = await _context.Evidencia.FindAsync(id);
        if (ev is null) return NotFound();

        // Borrar archivo físico si existe
        var abs = Path.Combine(_env.WebRootPath, ev.contenido_url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(abs))
            System.IO.File.Delete(abs);

        _context.Evidencia.Remove(ev);
        await _context.SaveChangesAsync();
        return Ok();
    }
    public string GetRandomString(int estado)
    {
        var obs = ObtenerObservaciones(estado);
        Random random = new Random();
        return obs[random.Next(9)];

    }
    public int GetRandomInt()
    {
        Random rnd = new Random();
        int[] opciones = { 6, 7, 10 };
        return opciones[rnd.Next(opciones.Length)];
    }
    public static List<int> RandomList(int cantidad)
    {
        var lista = new List<int>(capacity: cantidad);
        for (int i = 0; i < cantidad; i++)
            lista.Add(Random.Shared.Next(1, cantidad)); // 1..10 (11 es exclusivo)
        return lista;
    }

    private int? TryGetUserId()
    {
        var v = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(v, out var id) ? id : null;
    }

    public const int ESTADO_CUMPLE = 6;
    public const int ESTADO_NOCUMPLE = 7;
    public const int ESTADO_PARCIAL = 10;
    private static readonly Dictionary<int, List<string>> _obsPorEstado =
       new Dictionary<int, List<string>>
       {
            {
                ESTADO_CUMPLE,
                new List<string>
                {
                    "Se cumple el requisito según evidencia adjunta; control operativo y documentado.",
                    "Política vigente y alineada; última revisión dentro del periodo establecido.",
                    "Procedimiento implementado y aplicado de forma consistente en los casos muestreados.",
                    "Roles y responsabilidades definidos; aprobaciones registradas.",
                    "Registro y trazabilidad adecuados; no se observaron desviaciones.",
                    "Controles preventivos y detectivos efectivos; sin hallazgos.",
                    "Capacitación actualizada y evaluaciones de eficacia documentadas.",
                    "Indicadores/KPIs cumplen el umbral; seguimiento mensual evidenciado.",
                    "Gestión de cambios formalizada; no impacta el cumplimiento del artículo.",
                    "Evidencia técnica y legal suficiente; no se requieren acciones."
                }
            },
            {
                ESTADO_NOCUMPLE,
                new List<string>
                {
                    "No existe política/procedimiento que cubra el requisito; evidencia ausente.",
                    "Control no implementado o inefectivo; riesgo material no mitigado.",
                    "Documentación desactualizada; última revisión fuera de plazo.",
                    "No se registran aprobaciones ni trazabilidad de la actividad.",
                    "Evidencia insuficiente para sustentar cumplimiento; se solicita ampliación.",
                    "Incumplimiento reiterado en muestras revisadas; requiere corrección inmediata.",
                    "Capacitación no impartida o sin evidencia de evaluación.",
                    "No hay monitoreo/indicadores; ausencia de métricas de desempeño.",
                    "Hallazgo crítico: requisito legal no atendido; posible exposición sancionatoria.",
                    "Falta plan de acción; se requiere responsable y fecha de implementación."
                }
            },
            {
                ESTADO_PARCIAL,
                new List<string>
                {
                    "Cumplimiento parcial: política existe pero no se aplica en todos los casos.",
                    "Procedimiento implementado de forma incompleta; pasos clave sin evidencia.",
                    "Control opera en áreas seleccionadas; alcance debe ampliarse.",
                    "Documentación vigente pero con vacíos; requiere actualización puntual.",
                    "Registros disponibles, aunque la trazabilidad es intermitente.",
                    "Capacitación impartida, pendientes refuerzos o población residual.",
                    "Monitoreo con KPIs definidos, sin metas formalizadas o sin seguimiento.",
                    "Evidencia técnica presente; falta evidencia de aprobación formal.",
                    "Se identifican mejoras menores para fortalecer la eficacia del control.",
                    "Plan de acción definido; cumplimiento total estimado en el corto plazo."
                }
            }
       };

    public static List<string> ObtenerObservaciones(int idEstado)
        => _obsPorEstado.TryGetValue(idEstado, out var lista) ? lista : new List<string>();

}

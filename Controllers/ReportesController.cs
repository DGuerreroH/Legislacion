using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static LegislacionAPP.Models.CMMIReporteVM;

namespace LegislacionAPP.Controllers
{
    [Authorize(Roles = "Auditor,Gerente,Admin,Digitalizador")]
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;
        public ReportesController(AppDbContext context) => _context = context;

        
        [HttpGet]
        public async Task<IActionResult> ReporteCiclo(int cicloId)
        {
            var vm = await BuildVM(cicloId, esPdf: false);
            return View("ReporteCicloNuevo", vm);
        }

        [HttpGet]
        public async Task<IActionResult> ReporteCicloPdf(int cicloId)
        {
            var vm = await BuildVM(cicloId, esPdf: true);
            return new Rotativa.AspNetCore.ViewAsPdf("ReporteCicloNuevo", vm)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 12, 10)
            };
        }  

        private async Task<ReporteCicloVM> BuildVM(int cicloId, bool esPdf)
        {
            // Ciclo + Legislación + Empresa (+ País) + Auditor (+ País)
            var ciclo = await _context.CicloAuditoria
                .Include(c => c.id_legislacionNavigation)
                    .ThenInclude(l => l.id_empresaNavigation)
                        .ThenInclude(e => e.id_paisNavigation)
                .FirstOrDefaultAsync(c => c.id_ciclo_auditoria == cicloId);

            if (ciclo == null) throw new Exception("Ciclo no encontrado.");

            var auditor = await _context.Usuario
                .Include(u => u.id_paisNavigation)
                .FirstOrDefaultAsync(u => u.id_usuario == ciclo.id_usuario);

            var estados = await _context.Estado
                .Select(e => new { e.id_estado, e.codigo })
                .ToDictionaryAsync(x => x.id_estado, x => x.codigo);

            // Segmentos + evaluación vigente (por ciclo) + evidencias
            var segs = await _context.SegmentoLegislacion
                .Where(s => s.id_legislacion == ciclo.id_legislacion)
                .OrderBy(s => s.orden)
                .Select(s => new
                {
                    s.id_segmento_legislacion,
                    s.id_tipo_elemento,
                    tipo_elemento = s.id_tipo_elementoNavigation.nombre,
                    s.contenido,
                    s.tituloSegmento,
                    Eval = s.Evaluaciones
                        .Where(e => e.id_ciclo_auditoria == cicloId)
                        .OrderByDescending(e => e.id_evaluacion_segmento)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // precarga de evidencias por evaluación
            var evalIds = segs.Select(x => x.Eval?.id_evaluacion_segmento ?? 0)
                              .Where(id => id != 0).ToList();

            var evidencias = evalIds.Count == 0
                ? new List<Evidencia>()
                : await _context.Evidencia
                    .Where(ev => evalIds.Contains(ev.id_evaluacion_segmento))
                    .OrderBy(ev => ev.id_evidencia)
                    .ToListAsync();

            var evidPorEval = evidencias.GroupBy(e => e.id_evaluacion_segmento)
                                        .ToDictionary(g => g.Key, g => g.ToList());

            var vm = new ReporteCicloVM
            {
                Empresa = ciclo.id_legislacionNavigation.id_empresaNavigation!,
                Legislacion = ciclo.id_legislacionNavigation!,
                Auditor = auditor ?? new Usuario(),
                CicloAuditoria = ciclo,
                EsPdf = esPdf
            };

            int total = 0, cumple = 0, parcial = 0, nocumple = 0;

            foreach (var s in segs)
            {
                var seg = new SegmentoAuditableVM
                {
                    id_segmento_legislacion = s.id_segmento_legislacion,
                    id_tipo_elemento = s.id_tipo_elemento,
                    tipo_elemento = s.tipo_elemento,
                    contenido = s.contenido,
                    id_evaluacion_segmento = s.Eval?.id_evaluacion_segmento,
                    id_usuario_auditor = (int)await GetUsuarioIdActualAsync(),
                    observaciones = s.Eval?.comentario,
                    id_estado = s.Eval?.id_estado,
                    segmentoTitulo = s.tituloSegmento
                };               

                // Evidencias
                if (seg.id_evaluacion_segmento.HasValue &&
                    evidPorEval.TryGetValue(seg.id_evaluacion_segmento.Value, out var list))
                {
                    seg.Evidencias = list;
                }
                total++;     
                vm.Articulos.Add(seg);
            }

            vm.Total = total;
            vm.Cumple = cumple;
            vm.Parcial = parcial;
            vm.NoCumple = nocumple;
            vm.Porcentaje = total == 0 ? 0
                          : Math.Round(((cumple + parcial * 0.5m) / total) * 100m, 0);
            return vm;
        }

        private async Task<CMMIReporteVM?> BuildCMMIReporteVm(int empresaId, int evalId)
        {
            var eval = await _context.CMMIEvaluacion
                .Include(e => e.Empresa)
                .Include(e => e.id_UsuarioAuditorNavigation)
                .FirstOrDefaultAsync(e => e.id_evaluacion == evalId && e.id_empresa == empresaId);

            if (eval == null) return null;


            var cats = await _context.CMMIRespuesta
               .Where(r => r.id_evaluacion == eval.id_evaluacion)
               .Join(_context.CMMIPregunta,
                     r => r.id_ccmi_pregunta,
                     p => p.id_cmmi_pregunta,
                     (r, p) => new { r, p })
               .Join(_context.CCMICategoria,
                     rp => rp.p.id_ccmi_categoria,
                     c => c.id_ccmi_categoria,
                     (rp, c) => new { rp.r, rp.p, c })
               .GroupBy(x => new { x.c.id_ccmi_categoria, x.c.nombre, x.c.codigo, x.c.peso_categoria })
               .Select(g => new ItemCategoriaVM
               {
                   Codigo = g.Key.codigo,
                   Nombre = g.Key.nombre,                
                   Porcentaje = g.Sum(x => x.r.valor) / g.Count(),
               })
               .OrderBy(x => x.Codigo)
               .ToListAsync();

            // Trae categorías y preguntas
            var filas = await _context.CMMIRespuesta
                 .Where(r => r.id_evaluacion == eval.id_evaluacion)
                 .Join(_context.CMMIPregunta, r => r.id_ccmi_pregunta, p => p.id_cmmi_pregunta, (r, p) => new { r, p })
                 .Join(_context.CCMICategoria, rp => rp.p.id_ccmi_categoria, c => c.id_ccmi_categoria, (rp, c) => new { rp.r, rp.p, c })
                 .ToListAsync();

            // Respuestas previas si hay
            var resp = await _context.CMMIRespuesta
                .Where(r => r.id_evaluacion == evalId)
                .ToDictionaryAsync(r => r.id_ccmi_pregunta, r => r);                   
           
            decimal global = (decimal)(eval.puntaje_global ?? cats.DefaultIfEmpty().Average(c => c?.Porcentaje ?? 0m));
            global = Math.Round(global, 2);


            var vm = new CMMIReporteVM
            {
                id_empresa = empresaId,
                id_evaluacion = evalId,
                Empresa = eval.Empresa?.nombre ?? "",
                RepresentanteEmpresa = eval.Empresa?.representante ?? "",
                Auditor = (eval.id_UsuarioAuditorNavigation?.nombre + " " +
                           eval.id_UsuarioAuditorNavigation?.apellido).Trim(),
                fecha_inicio = eval.fecha_inicio,
                fecha_cierre = eval.fecha_cierre,
                PorcentajeGlobal = global,
                NivelGlobal = MapNivel(global),
                Categorias = null!, // se llena abajo
            };
            vm.Categorias = filas
            .GroupBy(x => new { x.c.id_ccmi_categoria, x.c.codigo, x.c.nombre, x.c.orden })
            .OrderBy(g => g.Key.orden)
            .Select(g => new CMMIReporteVM.CategoriaVM
            {
                Codigo = g.Key.codigo,
                Nombre = g.Key.nombre,
                Porcentaje = g.Sum(x => x.r.valor) / g.Count(),
                Preguntas = g.OrderBy(x => x.p.orden).Select(x => new CMMIReporteVM.PreguntaVM
                {
                    Codigo = x.p.codigo,
                    Texto = x.p.texto,
                    Valor = x.r.valor,
                    ValorPct = x.r.valor,
                    Comentario = x.r.observacion
                }).ToList()
            })
            .ToList();
            return vm;
        }
        private static string MapNivel(decimal pct) => pct switch
        {
            >= 90m => "Optimizado (Nivel 5)",
            >= 75m => "Gestionado (Nivel 4)",
            >= 60m => "Definido (Nivel 3)",
            >= 40m => "Inicial (Nivel 2)",
            _ => "Básico (Nivel 1)"
        };

        // Controllers/CMMIController.cs  (GET)
        [HttpGet]
        [Authorize(Roles = "Auditor,Admin")]
        [HttpGet]
        public async Task<IActionResult> Reporte(int empresaId, int evalId)
        {
            var vm = await BuildCMMIReporteVm(empresaId, evalId);
            if (vm == null) return NotFound();
            return View("ReporteCMMI", vm);   // misma vista que ya hiciste
        }      

        // HTML
        [HttpGet]
        public async Task<IActionResult> CMMI(int empresaId, int evalId)
        {
            var vm = await BuildCMMIReporteVm(empresaId, evalId);
            if (vm == null) return NotFound();
            vm.EsPdf = false;
            return View("ReporteCMMI", vm); // tu vista .cshtml
        }

        // PDF (usa la misma vista)
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> CMMIPdf(int empresaId, int evalId)
        {
            var vm = await BuildCMMIReporteVm(empresaId, evalId);
            if (vm == null) return NotFound();
            vm.EsPdf = true;

            return new Rotativa.AspNetCore.ViewAsPdf("ReporteCMMI", vm)
            {
                FileName = $"CMMI_{vm.Empresa}_{vm.fecha_inicio:yyyyMMdd}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(12, 12, 12, 12)
            };
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
}
using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LegislacionAPP.Controllers
{
    [Authorize(Roles = "Auditor,Admin,Gerente")]
    // Controllers/CMMIController.cs
    public class CMMIController : Controller
    {
        private readonly AppDbContext _db;
        // Helpers
        private static int NivelPorcentaje(decimal p) =>
            p >= 85 ? 5 : p >= 70 ? 4 : p >= 55 ? 3 : p >= 40 ? 2 : 1;
        public CMMIController(AppDbContext db) => _db = db;

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Auditor,Admin,Gerente")]
        public async Task<IActionResult> Evaluar(CMMIEvaluarVM vm, bool cerrar = false)
        {
            var eval = await _db.CMMIEvaluacion
                .FirstOrDefaultAsync(e => e.id_evaluacion == vm.id_evaluacion && e.id_empresa == vm.id_empresa);
            if (eval == null) return NotFound();

            // Upsert de respuestas
            var existentes = await _db.CMMIRespuesta
                .Where(r => r.id_evaluacion == vm.id_evaluacion)
                .ToDictionaryAsync(r => r.id_ccmi_pregunta, r => r);

            foreach (var c in vm.Categorias)
            {
                foreach (var p in c.Preguntas)
                {
                    if (!existentes.TryGetValue(p.id_pregunta, out var r))
                    {
                        r = new CMMIRespuesta
                        {
                            id_evaluacion = vm.id_evaluacion,
                            id_ccmi_pregunta = p.id_pregunta,
                            valor = (byte)(p.valor ?? 0), 
                            observacion = p.comentario ?? ""
                        };
                        _db.CMMIRespuesta.Add(r);
                        existentes[p.id_pregunta] = r;
                    }
                    r.valor = (byte)(p.valor ?? 0);
                    r.observacion = p.comentario;
                }
            }

            await _db.SaveChangesAsync();

            // Recalcula y guarda en el header de evaluación
            var (porc, nivel) = Calcular(vm);
            eval.puntaje_global = porc;
            eval.nivel_madurez = (byte)nivel;
            if(cerrar == true)
            {
                eval.id_estado = (int)EstadoCodigo.Inactiva;
                eval.fecha_cierre = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();

            if (cerrar == true)
            {
                TempData["msg"] = "Evaluación cerrada.";
                return RedirectToAction(nameof(Index), new { empresaId = vm.id_empresa});
            }
            else {
                TempData["msg"] = "Evaluación guardada.";
                return RedirectToAction(nameof(Evaluar), new { empresaId = vm.id_empresa, evalId = vm.id_evaluacion });
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> Reporte(int evalId)
        {
            // Arma tu VM de reporte y retorna la vista:
            return View(/* vm */); // => Views/CMMI/Reporte.cshtml
        }
        private async Task<int?> GetUsuarioIdActualAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email)) return null;

            return await _db.Usuario
                .Where(u => u.correo == email)
                .Select(u => (int?)u.id_usuario)
                .FirstOrDefaultAsync();
        }
        // GET: /CMMI/Index?empresaId=123
        [HttpGet]
        [Authorize(Roles = "Auditor,Admin,Gerente")]
        public async Task<IActionResult> Index(int empresaId)
        {
            var emp = await _db.Empresa.FindAsync(empresaId);
            if (emp is null) return NotFound();

            var filas = await _db.CMMIEvaluacion
                .Where(e => e.id_empresa == empresaId)
                .Include(e => e.id_estadoNavigation)
                .Include(e => e.id_UsuarioAuditorNavigation)
                .OrderByDescending(e => e.fecha_inicio)
                .Select(e => new CMMIEvalRowVM
                {
                    id_evaluacion = e.id_evaluacion,
                    fecha_inicio = e.fecha_inicio,
                    fecha_cierre = e.fecha_cierre,
                    Auditor = ((e.id_UsuarioAuditorNavigation.nombre + " " + e.id_UsuarioAuditorNavigation.apellido).Trim()),
                    id_estado = e.id_estado,
                    Estado = e.id_estadoNavigation.nombre == "Inactiva" ? "Cerrado": e.id_estadoNavigation.nombre,
                    Porcentaje = e.puntaje_global ?? 0m,
                    Nivel = e.nivel_madurez ?? 0
                })
                .ToListAsync();

            // Fallback de nivel si aún no está calculado:
            foreach (var r in filas)
                if (r.Nivel == 0) r.Nivel = NivelPorcentaje(r.Porcentaje);

            var vm = new CMMIIndexVM
            {
                id_empresa = empresaId,
                Empresa = emp.nombre,
                Evaluaciones = filas
            };

            return View(vm); // Views/CMMI/Index.cshtml
        }

        // POST: /CMMI/Nueva (crea o continúa borrador y manda a Evaluar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Auditor,Admin")]
        public async Task<IActionResult> Nueva(int empresaId)
        {
            // Revisa si ya hay una abierta (borrador o en revisión)
            var abierta = await _db.CMMIEvaluacion
                .Where(e => e.id_empresa == empresaId &&
                            (e.id_estado == (int)EstadoCodigo.Borrador ||
                             e.id_estado == (int)EstadoCodigo.EnRevision))
                .OrderByDescending(e => e.fecha_inicio)
                .FirstOrDefaultAsync();

            if (abierta == null)
            {
                // Crea borrador
                abierta = new CMMIEvaluacion
                {
                    id_empresa = empresaId,
                    id_usuario_auditor = await GetUsuarioIdActualAsync() ?? 0,
                    id_estado = (int)EstadoCodigo.Borrador,
                    fecha_inicio = DateTime.UtcNow,
                    puntaje_global = 0m,
                    nivel_madurez = 0
                };
                _db.CMMIEvaluacion.Add(abierta);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Evaluar", "CMMI", new { empresaId, evalId = abierta.id_evaluacion });
        }

        [HttpGet]
        [Authorize(Roles = "Auditor,Admin")]
        public async Task<IActionResult> Evaluar(int empresaId, int evalId)
        {
            var eval = await _db.CMMIEvaluacion
                .AsNoTracking()
                .Include(e => e.Empresa)
                .Include(e => e.id_UsuarioAuditorNavigation)
                .FirstOrDefaultAsync(e => e.id_evaluacion == evalId && e.id_empresa == empresaId);

            if (eval == null) return NotFound();

            var cats = await _db.CCMICategoria
                .AsNoTracking()
                .Where(c => c.activo)
                .OrderBy(c => c.orden)
                .Select(c => new {
                    c.id_ccmi_categoria,
                    c.nombre,
                    c.orden,
                    Pregs = c.Preguntas
                        .Where(p => p.activo)
                        .OrderBy(p => p.orden)
                        .Select(p => new {
                            p.id_cmmi_pregunta,
                            p.codigo,
                            p.texto,
                            p.peso_pregunta,
                            p.es_critica
                        })
                        .ToList()
                })
                .ToListAsync();

            var resp = await _db.CMMIRespuesta
                .AsNoTracking()
                .Where(r => r.id_evaluacion == evalId)
                .Select(r => new
                {
                    r.id_ccmi_pregunta,
                    r.valor,
                    r.observacion,
                    r.evidencia_url
                })
                .ToDictionaryAsync(x => x.id_ccmi_pregunta);

            var vm = new CMMIEvaluarVM
            {
                id_empresa = empresaId,
                id_evaluacion = evalId,
                Empresa = eval.Empresa?.nombre ?? "",
                Auditor = ((eval.id_UsuarioAuditorNavigation?.nombre + " " + eval.id_UsuarioAuditorNavigation?.apellido)?.Trim()),
                fecha_inicio = eval.fecha_inicio,
                fecha_cierre = eval.fecha_cierre
            };

            foreach (var c in cats)
            {
                var catVM = new CMMICategoriaEvalVM
                {
                    id_categoria = c.id_ccmi_categoria,
                    nombre = c.nombre,
                    orden = c.orden
                };

                foreach (var p in c.Pregs)
                {
                    // Trae respuesta si existe
                    resp.TryGetValue(p.id_cmmi_pregunta, out var r);
                    catVM.Preguntas.Add(new CMMIPreguntaEvalVM
                    {
                        id_pregunta = p.id_cmmi_pregunta,
                        codigo = p.codigo,
                        texto = p.texto,
                        peso = p.peso_pregunta,
                        es_critica = p.es_critica,
                        valor = r?.valor,
                        comentario = r?.observacion,
                    });
                }
                vm.Categorias.Add(catVM);
            }

            // Resumen
            (vm.PorcentajeGlobal, vm.NivelGlobal) = Calcular(vm);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Certificado(int empresaId)
        {
            var vm = await BuildCertVM(empresaId);
            if (vm == null)
            {
                TempData["err"] = "No existe una evaluación CMMI cerrada para esta empresa.";
                return RedirectToAction("Index", new { empresaId });
            }
            vm.EsPdf = false;
            return View("Certificado", vm); // HTML normal
        }

        [HttpGet]
        public async Task<IActionResult> CertificadoPdf(int empresaId)
        {
            var vm = await BuildCertVM(empresaId);
            if (vm == null)
            {
                TempData["err"] = "No existe una evaluación CMMI cerrada para esta empresa.";
                return RedirectToAction("Index", new { empresaId });
            }
            vm.EsPdf = true;
            return new Rotativa.AspNetCore.ViewAsPdf("Certificado", vm)
            {
                FileName = $"CMMI_{vm.Empresa}_{vm.FechaEmision:yyyyMMdd}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(12, 12, 12, 12)
            };
            
        }

        private async Task<CMMICertificadoVM> BuildCertVM(int empresaId)
        {
            var eval = await _db.CMMIEvaluacion
                .Where(e => e.id_empresa == empresaId && e.fecha_cierre != null)
                .OrderByDescending(e => e.fecha_cierre)
                .FirstOrDefaultAsync();

            if (eval == null) return null!;

            var empresa = await _db.Empresa
                .Include(e => e.id_paisNavigation)
                .FirstOrDefaultAsync(e => e.id_empresa == empresaId);

            var auditor = await _db.Usuario
                .FirstOrDefaultAsync(u => u.id_usuario == eval.id_usuario_auditor);

            var cats = await _db.CMMIRespuesta
                .Where(r => r.id_evaluacion == eval.id_evaluacion)
                .Join(_db.CMMIPregunta, r => r.id_ccmi_pregunta, p => p.id_cmmi_pregunta, (r, p) => new { r, p })
                .Join(_db.CCMICategoria, rp => rp.p.id_ccmi_categoria, c => c.id_ccmi_categoria, (rp, c) => new { rp.r, rp.p, c })
                .GroupBy(x => new { x.c.id_ccmi_categoria, x.c.nombre, x.c.codigo, x.c.peso_categoria })
                .Select(g => new ItemCategoriaVM
                {
                    Codigo = g.Key.codigo,
                    Nombre = g.Key.nombre,
                    Porcentaje = g.Sum(x => x.r.valor) / g.Count(),
                })
                .OrderBy(x => x.Codigo)
                .ToListAsync();

            decimal pctGlobal = (decimal)(eval.puntaje_global ?? cats.DefaultIfEmpty().Average(c => c?.Porcentaje ?? 0m));
            pctGlobal = Math.Round(pctGlobal, 2);

            return new CMMICertificadoVM
            {
                Empresa = empresa!,
                EmpresaPais = empresa?.id_paisNavigation?.nombre,
                Evaluacion = eval,
                Auditor = auditor!,
                PorcentajeGlobal = pctGlobal,
                NivelGlobal = MapNivel(pctGlobal),
                Categorias = cats,
                CertId = $"CMMI-{empresaId}-{eval.id_evaluacion}",
                FechaEmision = eval.fecha_cierre!.Value,
                FechaVencimiento = eval.fecha_cierre!.Value.AddYears(1),
                LogoUrl = empresa?.logo,
                EsPdf = false
            };
        }

        private static (decimal porc, int nivel) Calcular(CMMIEvaluarVM vm)
        {
            decimal sumPeso = 0, sumScore = 0;
            foreach (var c in vm.Categorias)
                foreach (var p in c.Preguntas)
                    if (p.valor.HasValue)
                    {
                        var v = Math.Clamp(p.valor.Value, 0, 100);
                        sumPeso += p.peso;
                        sumScore += p.peso * v;
                    }
            int totalPreguntas = vm.Categorias.Sum(c => c.Preguntas.Count);
            var porc = (sumPeso > 0 ? sumScore / totalPreguntas : 0);
            int nivel = porc >= 85 ? 5 : porc >= 70 ? 4 : porc >= 55 ? 3 : porc >= 40 ? 2 : 1;
            return (Math.Round(porc, 2), nivel);
        }
        private static string MapNivel(decimal pct) => pct switch
        {
            >= 90m => "Optimizado (Nivel 5)",
            >= 75m => "Gestionado (Nivel 4)",
            >= 60m => "Definido (Nivel 3)",
            >= 40m => "Inicial (Nivel 2)",
            _ => "Básico (Nivel 1)"
        };
    }


}

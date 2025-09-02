using LegislacionAPP.Data;
using LegislacionAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LegislacionAPP.Controllers
{
    //[Authorize(Roles = RolNames.Gerente)]
    public class GerenciaController : Controller
    {
        private readonly AppDbContext _context;

        public GerenciaController(AppDbContext ctx) => _context = ctx;

        [HttpGet]
        public async Task<IActionResult> HistorialPdf(int legislacionId)
        {
            var l = await _context.Legislacion
                .Include(x => x.id_empresaNavigation)
                .FirstOrDefaultAsync(x => x.id_legislacion == legislacionId);

            if (l is null) return NotFound();

            var ciclos = await _context.CicloAuditoria
                .Where(c => c.id_legislacion == legislacionId)
                .OrderByDescending(c => c.fecha_inicio)
                .Select(c => new CicloResumenVM
                {
                    id_ciclo_auditoria = c.id_ciclo_auditoria,
                    fecha_inicio = c.fecha_inicio,
                    fecha_cierre = c.fecha_cierre,
                    articulos_totales = c.articulos_totales,
                    articulos_aprobados = c.articulos_aprobados,
                    porcentaje_avance = (decimal)c.porcentaje_aprobado                
                })
                .ToListAsync();

            var vm = new HistorialLegislacionVM
            {
                Empresa = l.id_empresaNavigation.nombre,
                Legislacion = l.titulo,
                Ambito = l.id_ambito_aplicacionNavigation?.nombre,
                Ciclos = ciclos
            };

            return new Rotativa.AspNetCore.ViewAsPdf("HistorialPDF", vm)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.Letter
            };
        }


        [HttpGet]
        public async Task<IActionResult> CertificadoPdf(int legislacionId)
        {
            var l = await _context.Legislacion
                .Include(x => x.id_empresaNavigation)
                 .Include(x => x.id_paisNavigation)
                .FirstOrDefaultAsync(x => x.id_legislacion == legislacionId);
            if (l is null) return NotFound();

            var cicloOk = await _context.CicloAuditoria
                .Where(c => c.id_legislacion == legislacionId && c.fecha_cierre != null)
                .OrderByDescending(c => c.fecha_cierre)
                .FirstOrDefaultAsync();

            if (cicloOk is null || cicloOk.porcentaje_aprobado != 100)
                return BadRequest("La legislación no tiene un ciclo cerrado al 100%.");

            var vm = new CertificadoVM
            {
                Empresa = l.id_empresaNavigation.nombre,
                Pais = l.id_paisNavigation?.nombre,
                Legislacion = l.titulo,
                FechaCierre = cicloOk.fecha_cierre!.Value,
                Porcentaje = cicloOk.porcentaje_aprobado,
                Folio = $"CERT-{legislacionId}-{cicloOk.id_ciclo_auditoria:D6}"
            };

            return new Rotativa.AspNetCore.ViewAsPdf("CertificadoPDF", vm)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.Letter
            };
        }
    }

}

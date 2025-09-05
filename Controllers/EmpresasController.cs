using LegislacionAPP.Common.Enum;
using LegislacionAPP.Data;
using LegislacionAPP.Models.Helpers;
using LegislacionAPP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

//[Authorize] // cualquier usuario autenticado
public class EmpresasController : Controller
{
    private readonly AppDbContext _db;
    public EmpresasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index(int empresaId)
    {
        var usuarioId = await GetUsuarioIdActualAsync();
        var empresas = new List<EmpresaCardVM>();
        if ((User.IsInRole(Roles.Admin) || (User.IsInRole(Roles.Auditor))))
        {
            empresas = await _db.Empresa                
                .Select(ue => new EmpresaCardVM
                {
                    id_empresa = ue.id_empresa,
                    nombre = ue.nombre,
                    logo = ue.logo,
                    pais = ue.id_paisNavigation.nombre,
                    estado = ue.id_estadoNavigation.nombre,
                    rolAsignado = ue.nombre
                })
                .OrderBy(e => e.nombre)
                .ToListAsync();
        }
        else
        {
            empresas = await _db.UsuarioEmpresa
                .Where(ue => ue.id_usuario == usuarioId)
                .Select(ue => new EmpresaCardVM
                {
                    id_empresa = ue.id_empresa,
                    nombre = ue.id_empresaNavigation.nombre,
                    logo = ue.id_empresaNavigation.logo,
                    pais = ue.id_empresaNavigation.id_paisNavigation.nombre,
                    estado = ue.id_empresaNavigation.id_estadoNavigation.nombre,
                    rolAsignado = ue.id_rolNavigation.nombre
                })
                .OrderBy(e => e.nombre)
                .ToListAsync();
        }
        return View(empresas);
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var usuarioId = await GetUsuarioIdActualAsync();

        var empresa = await _db.UsuarioEmpresa
            .Where(ue => ue.id_usuario == usuarioId && ue.id_empresa == id)
            .Select(ue => ue.id_empresaNavigation)
            .Include(e => e.id_estadoNavigation)
            .Include(e => e.id_paisNavigation)
            .FirstOrDefaultAsync();

        if (empresa is null) return Forbid();

        return View(empresa); 
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
}

using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LegislacionAPP.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(AppDbContext context, ILogger<UsuarioController> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IActionResult> Index(string? q)
        {
            var query = _context.Usuario
                .AsNoTracking()
                .OrderBy(u => u.nombre).ThenBy(u => u.apellido)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(u =>
                    u.nombre.Contains(q) ||
                    u.apellido.Contains(q) ||
                    u.correo.Contains(q));
            }

            var datos = await query.Select(u => new
            {
                u.id_usuario,
                u.nombre,
                u.apellido,
                u.correo,
                u.id_estado,
                EstadoNombre = u.id_estadoNavigation.nombre,
                RolNombre = u.id_rolNavigation.nombre,

                Empresas = u.UsuarioEmpresas
                    .Select(ue => ue.id_empresaNavigation.nombre)
                    .OrderBy(n => n)
                    .ToList()
            })
            .ToListAsync();

            var model = datos.Select(u => new UsuariosIndexVM
            {
                id_usuario = u.id_usuario,
                Nombre = u.nombre,
                Apellido = u.apellido,
                Correo = u.correo,
                IdEstado = u.id_estado,
                EstadoNombre = u.EstadoNombre,
                RolNombre = u.RolNombre,
                EmpresasLista = (u.Empresas ?? new List<string>())
                                    .Where(n => !string.IsNullOrWhiteSpace(n))
                                    .Any()
                                ? string.Join(", ", u.Empresas.Where(n => !string.IsNullOrWhiteSpace(n)))
                                : null
            })
            .ToList();

            return View(model);
        }

        // GET: Crear
        public async Task<IActionResult> Create()
        {
            var (empresas, roles, paises) = await GetCatalogos();
            var vm = new UsuarioUpsertVM { Empresas = empresas, Roles = roles , Paises = paises };
            vm.Asignaciones.Add(new UsuarioEmpresaAsignacionVM());
            return View("Upsert", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioUpsertVM vm)
        {
            var (empresas, roles, paises) = await GetCatalogos();
            vm.Empresas = empresas; vm.Roles = roles; vm.Paises = paises;

            if (await _context.Usuario.AnyAsync(u => u.correo == vm.correo))
                ModelState.AddModelError(nameof(vm.correo), "Ya existe un usuario con este correo.");

            if (!ModelState.IsValid)
                return View("Upsert", vm);

            try
            {
                var user = new Usuario
                {
                    nombre = vm.nombre,
                    apellido = vm.apellido,
                    correo = vm.correo,
                    contrasena_hash = string.IsNullOrWhiteSpace(vm.password) ? "123456" : vm.password, // TODO: hash si aplica
                    id_rol = vm.id_rol,
                    id_estado = vm.id_estado,
                    id_pais = vm.id_pais,
                    fecha_creacion = DateTime.UtcNow
                };

                _context.Usuario.Add(user);
                await _context.SaveChangesAsync();

                var asigns = vm.Asignaciones?
                    .Where(a => a != null && a.id_empresa > 0 && a.id_rol > 0)
                    .GroupBy(a => a.id_empresa)
                    .Select(g => g.First())
                    .ToList() ?? new();

                foreach (var a in asigns)
                {
                    _context.UsuarioEmpresa.Add(new UsuarioEmpresa
                    {
                        id_usuario = user.id_usuario,
                        id_empresa = a.id_empresa,
                        id_rol = a.id_rol,
                        fecha_asignacion = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();

                TempData["ok"] = "Usuario creado y asignaciones guardadas.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando usuario");
                ModelState.AddModelError(string.Empty, "Error al guardar. Revisa los datos.");
                return View("Upsert", vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.id_usuario == id);
            if (user == null) return NotFound();

            var asigns = await _context.UsuarioEmpresa
                .AsNoTracking()
                .Where(ue => ue.id_usuario == id)
                .Select(ue => new UsuarioEmpresaAsignacionVM
                {
                    id_usuario_empresa = ue.id_usuario_empresa,
                    id_empresa = ue.id_empresa,
                    id_rol = 1
                }).ToListAsync();

            var (empresas, roles, paises) = await GetCatalogos();

            var vm = new UsuarioUpsertVM
            {
                id_usuario = user.id_usuario,
                nombre = user.nombre,
                apellido = user.apellido,
                correo = user.correo,
                id_rol = user.id_rol,
                id_estado = user.id_estado,
                id_pais = user.id_pais,
                Asignaciones = asigns,
                Empresas = empresas,
                Roles = roles,
                Paises = paises

            };

            if (vm.Asignaciones.Count == 0) vm.Asignaciones.Add(new UsuarioEmpresaAsignacionVM());
            return View("Upsert", vm);
        }

        // POST: Editar
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioUpsertVM vm)
        {
            var (empresas, roles, paises) = await GetCatalogos();
            vm.Empresas = empresas; vm.Roles = roles; vm.Paises = paises;

            var user = await _context.Usuario.FirstOrDefaultAsync(u => u.id_usuario == vm.id_usuario);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
                return View("Upsert", vm);

            try
            {
                // actualizar usuario
                user.nombre = vm.nombre;
                user.apellido = vm.apellido;
                user.correo = vm.correo;
                if (!string.IsNullOrWhiteSpace(vm.password))
                    user.contrasena_hash = vm.password; 
                user.id_rol = vm.id_rol;
                user.id_estado = vm.id_estado;
                user.id_pais = vm.id_pais;
                user.fecha_actualizacion = DateTime.UtcNow;

                // sincronizar asignaciones
                var entradas = vm.Asignaciones?
                    .Where(a => a != null && a.id_empresa > 0)
                    .GroupBy(a => a.id_empresa)
                    .Select(g => g.First())
                    .ToList() ?? new();

                var actuales = await _context.UsuarioEmpresa
                    .Where(ue => ue.id_usuario == user.id_usuario)
                    .ToListAsync();

                // borrar las que ya no vienen
                var toDelete = actuales.Where(c => !entradas.Any(e => e.id_empresa == c.id_empresa)).ToList();
                _context.UsuarioEmpresa.RemoveRange(toDelete);

                // upsert para las que vienen
                foreach (var e in entradas)
                {
                    var exist = actuales.FirstOrDefault(c => c.id_empresa == e.id_empresa);
                    if (exist == null)
                    {
                        _context.UsuarioEmpresa.Add(new UsuarioEmpresa
                        {
                            id_usuario = user.id_usuario,
                            id_empresa = e.id_empresa,
                            id_rol = 1,
                            fecha_asignacion = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        exist.id_rol = 1;
                        exist.fecha_asignacion = DateTime.UtcNow;
                        _context.UsuarioEmpresa.Update(exist);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["ok"] = "Usuario actualizado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editando usuario");
                ModelState.AddModelError(string.Empty, "Error al guardar. Revisa los datos.");
                return View("Upsert", vm);
            }
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                usuario.id_estado = (int)EstadoCodigo.Inactiva;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.id_usuario == id);
        }
        // Util: catálogos
        private async Task<(IEnumerable<SelectListItem> Empresas, IEnumerable<SelectListItem> Roles, IEnumerable<SelectListItem> Paises)> GetCatalogos()
        {
            var empresas = await _context.Empresa
                .AsNoTracking()
                //.Where(e => e.id_estado == 2) // filtrar por solo activas
                .OrderBy(e => e.nombre)
                .Select(e => new SelectListItem { Value = e.id_empresa.ToString(), Text = e.nombre })
                .ToListAsync();

            var roles = await _context.Rol
                .AsNoTracking()
                .OrderBy(r => r.id_rol)
                .Select(r => new SelectListItem { Value = r.id_rol.ToString(), Text = r.nombre })
                .ToListAsync();

            var paises = await _context.Pais
           .AsNoTracking()
           .OrderBy(r => r.id_pais)
           .Select(r => new SelectListItem { Value = r.id_pais.ToString(), Text = r.nombre })
           .ToListAsync();

            return (empresas, roles,paises);
        }
    }
}

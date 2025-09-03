using LegislacionAPP.Common.Enum;
using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LegislacionAPP.Controllers
{
    [Authorize(Roles = "Admin,Gerente")]

    public class EmpresaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public EmpresaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Gerente")]
        public async Task<IActionResult> Index()
        {
            var user = await GetUsuarioIdActualAsync();
            if (User.IsInRole(Roles.Admin))
            {
                var items = await _context.Empresa
                   .AsNoTracking()
                   .Include(e => e.id_paisNavigation)
                   .Include(e => e.id_sectorNavigation)
                   .Include(e => e.id_estadoNavigation)
                   .OrderBy(e => e.nombre)
                   .Select(e => new EmpresaIndexRowVM
                   {
                       id_empresa = e.id_empresa,
                       nombre = e.nombre,
                       logo = e.logo,
                       pais = e.id_paisNavigation.nombre,
                       sector = e.id_sectorNavigation.nombre,
                       estado = e.id_estadoNavigation.codigo,
                       fecha_creacion = e.fecha_creacion,
                       id_estado = e.id_estado

                   })
                   .ToListAsync();

                return View(items);
            }
            else {
                var items = await _context.Empresa
                .AsNoTracking()
                .Where(e => e.id_estado != (int)EstadoCodigo.Inactiva)
                .Where(e => _context.UsuarioEmpresa
                    .Any(ue => ue.id_empresa == e.id_empresa && ue.id_usuario == user))
                .OrderBy(e => e.nombre)
                .Select(e => new EmpresaIndexRowVM
                {
                    id_empresa = e.id_empresa,
                    nombre = e.nombre,
                    logo = e.logo,
                    pais = e.id_paisNavigation.nombre,
                    sector = e.id_sectorNavigation.nombre,
                    estado = e.id_estadoNavigation.codigo,
                    fecha_creacion = e.fecha_creacion,
                    id_estado = e.id_estado
                })
                .ToListAsync();
                return View(items);
            }
               
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new EmpresaFormVM
            {
                id_estado = (int)EstadoCodigo.Activa, 
                fecha_creacion = DateTime.UtcNow
            };
            await FillSelects(vm);
            ViewData["Title"] = "Crear empresa";
            return View("Edit", vm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpresaFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                FillSelects(vm);
                return View(vm);
            }

            var entity = new Empresa
            {
                nombre = vm.nombre,
                representante = vm.representante,
                nit = vm.nit,
                id_pais = vm.id_pais,
                id_sector = vm.id_sector,
                id_estado = (int)EstadoCodigo.Activa,
                fecha_creacion = DateTime.UtcNow,
                fecha_actualizacion = DateTime.UtcNow                
            };

            _context.Add(entity);
            await _context.SaveChangesAsync(); 

            if (vm.LogoFile != null)
            {
                entity.logo = await SaveLogoAsync(vm.LogoFile, entity.id_empresa, null);
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }

            TempData["msg"] = "Empresa creada.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? empresaId)
        {
            if (empresaId == null) return NotFound();

            var e = await _context.Empresa.AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_empresa == empresaId);
            if (e == null) return NotFound();

            var vm = new EmpresaFormVM
            {
                id_empresa = e.id_empresa,
                nombre = e.nombre,
                logo = e.logo,
                id_pais = e.id_pais,
                id_sector = e.id_sector,
                id_estado = e.id_estado,
                fecha_creacion = e.fecha_creacion,
                nit = e.nit,
                representante = e.representante,

            };
            await FillSelects(vm);
            ViewData["Title"] = "Editar empresa";

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpresaFormVM vm, IFormFile? LogoFile)
        {            
            if (String.IsNullOrEmpty(vm.nombre))
            {
                FillSelects(vm);
                return View(vm);
            }

            string? newLogoUrl = vm.logo;
            if (LogoFile != null && LogoFile.Length > 0)
            {
                var logosPath = Path.Combine(_env.WebRootPath, "logos");
                Directory.CreateDirectory(logosPath);
                var fileName = $"emp_{(vm.id_empresa == 0 ? "new" : vm.id_empresa)}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{Path.GetExtension(LogoFile.FileName)}";
                var savePath = Path.Combine(logosPath, fileName);
                using (var fs = System.IO.File.Create(savePath))
                    await LogoFile.CopyToAsync(fs);
                newLogoUrl = $"/logos/{fileName}";
            }
            if (vm.id_empresa == 0)
            {
                // Crear
                var ent = new Empresa
                {
                    id_estado = vm.id_estado,
                    logo = newLogoUrl,
                    nombre = vm.nombre,
                    representante = vm.representante,
                    nit = vm.nit,
                    id_pais = vm.id_pais,
                    id_sector = vm.id_sector,
                    fecha_creacion = DateTime.UtcNow
                };
                _context.Empresa.Add(ent);
                await _context.SaveChangesAsync();
                TempData["ok"] = "Empresa creada correctamente.";
                return RedirectToAction(nameof(Edit), new { empresaId = ent.id_empresa });
            }
            else
            {
                // Update
                var ent = await _context.Empresa.FindAsync(vm.id_empresa);
                if (ent == null) return NotFound();

                ent.nombre = vm.nombre;
                ent.representante = vm.representante;
                ent.nit = vm.nit;
                ent.id_pais = vm.id_pais;
                ent.id_sector = vm.id_sector;
                ent.id_estado = vm.id_estado;
                if (newLogoUrl != vm.logo && newLogoUrl != null)
                {
                    ent.logo = newLogoUrl;
                }

                await _context.SaveChangesAsync();
                TempData["ok"] = "Empresa actualizada.";
                return RedirectToAction(nameof(Edit), new { empresaId = ent.id_empresa });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int empresaId)
        {
            var empresa = await _context.Empresa
                .Include(e => e.id_estadoNavigation)
                .Include(e => e.id_paisNavigation)
                .FirstOrDefaultAsync(m => m.id_empresa == empresaId);
            if (empresa == null) return NotFound();
            return View(empresa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int empresaId)
        {
            var empresa = await _context.Empresa.FindAsync(empresaId);
            if(empresa != null)
            {
                empresa.id_estado = (int)EstadoCodigo.Inactiva;  
                _context.Empresa.Update(empresa);
                _context.SaveChanges();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task FillSelects(EmpresaFormVM vm)
        {
            vm.Paises = await _context.Pais
             .OrderBy(p => p.nombre)
             .Select(p => new SelectListItem { Value = p.id_pais.ToString(), Text = p.nombre, Selected = (p.id_pais == vm.id_pais) })
             .ToListAsync();

            vm.Ambito = await _context.AmbitoAplicacion
                .OrderBy(s => s.nombre)
                .Select(s => new SelectListItem { Value = s.id_ambito_aplicacion.ToString(), Text = s.nombre, Selected = (s.id_ambito_aplicacion == vm.id_sector) })
                .ToListAsync();

            vm.Estados = await _context.Estado
                .OrderBy(e => e.nombre)
                .Select(e => new SelectListItem { Value = e.id_estado.ToString(), Text = e.nombre, Selected = (e.id_estado == vm.id_estado) })
                .ToListAsync();
            vm.Sectores = await _context.Sector
                .OrderBy(s => s.nombre)
                .Select(s => new SelectListItem { Value = s.id_sector.ToString(), Text = s.nombre, Selected = (s.id_sector == vm.id_sector) })
                .ToListAsync();
        }

        private async Task<string?> SaveLogoAsync(IFormFile file, int empresaId, string? oldPath)
        {
            // Filtro de archivos permitidos
            var allowed = new[] { ".png", ".jpg", ".jpeg", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext) || file.Length > 2_000_000) return oldPath;

            var folder = Path.Combine(_env.WebRootPath, "uploads", "logos");
            Directory.CreateDirectory(folder);

            var fileName = $"{empresaId}_{Guid.NewGuid():N}{ext}";
            var dest = Path.Combine(folder, fileName);
            using (var fs = new FileStream(dest, FileMode.Create))
                await file.CopyToAsync(fs);

            // Eliminacion de logo anterior
            if (!string.IsNullOrEmpty(oldPath))
            {
                var fullOld = Path.Combine(_env.WebRootPath, oldPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(fullOld)) try { System.IO.File.Delete(fullOld); } catch { /* ignore */ }
            }

            return $"/uploads/logos/{fileName}";
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

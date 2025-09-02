using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegislacionAPP.Controllers
{
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
        public async Task<IActionResult> Index()
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
                    fecha_creacion = e.fecha_creacion
                })
                .ToListAsync();

            return View(items);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new EmpresaFormVM();
            FillSelects(vm);
            return View(vm);
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

                Paises = new SelectList(await _context.Pais.OrderBy(p => p.nombre).ToListAsync(), "id_pais", "nombre", e.id_pais),
                Sectores = new SelectList(await _context.Sector.OrderBy(s => s.nombre).ToListAsync(), "id_sector", "nombre", e.id_sector),
                Estados = new SelectList(await _context.Estado.OrderBy(x => x.codigo).ToListAsync(), "id_estado", "codigo", e.id_estado)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpresaFormVM model, IFormFile? LogoFile)
        {
            if (model.id_empresa == 0) return NotFound();

            if (String.IsNullOrEmpty(model.nombre))
            {
                model.Paises = new SelectList(await _context.Pais.OrderBy(p => p.nombre).ToListAsync(), "id_pais", "nombre", model.id_pais);
                model.Sectores = new SelectList(await _context.Sector.OrderBy(s => s.nombre).ToListAsync(), "id_sector", "nombre", model.id_sector);
                model.Estados = new SelectList(await _context.Estado.OrderBy(x => x.codigo).ToListAsync(), "id_estado", "codigo", model.id_estado);
                return View(model);
            }

            var e = await _context.Empresa.FirstOrDefaultAsync(x => x.id_empresa == model.id_empresa);
            if (e == null) return NotFound();

            // Actualiza campos
            e.nombre = model.nombre;
            e.id_pais = model.id_pais;
            e.id_sector = model.id_sector;
            e.id_estado = model.id_estado;
            e.representante = model.representante;
            e.nit = model.nit;

            // Subida de logo
            if (LogoFile != null && LogoFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "logos");
                Directory.CreateDirectory(uploads);

                var ext = Path.GetExtension(LogoFile.FileName);
                var fileName = $"{e.id_empresa}_{Guid.NewGuid():N}{ext}";
                var dest = Path.Combine(uploads, fileName);

                using var fs = new FileStream(dest, FileMode.Create);
                await LogoFile.CopyToAsync(fs);

                e.logo = $"/uploads/logos/{fileName}";
            }

            await _context.SaveChangesAsync();
            TempData["msg"] = "Empresa actualizada correctamente.";
            return RedirectToAction(nameof(Index));
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
            if (empresa != null) _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void FillSelects(EmpresaFormVM vm)
        {
            vm.Paises = new SelectList(_context.Pais.AsNoTracking(), "id_pais", "nombre", vm.id_pais);
            vm.Sectores = new SelectList(_context.Sector.AsNoTracking(), "id_sector", "nombre", vm.id_sector);
            vm.Estados = new SelectList(_context.Estado.AsNoTracking(), "id_estado", "codigo", vm.id_estado);
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
    }
}

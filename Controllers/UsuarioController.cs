using LegislacionAPP.Common.Enums;
using LegislacionAPP.Data;
using LegislacionAPP.Models;
using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LegislacionAPP.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? q, int? id_estado, int? id_pais)
        {
            var query = _context.Usuario
                .Include(u => u.id_estadoNavigation)
                .Include(u => u.id_paisNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(u =>
                    (u.nombre ?? "").Contains(q) ||
                    (u.apellido ?? "").Contains(q) ||
                    (u.correo ?? "").Contains(q));
            }
            if (id_estado.HasValue) query = query.Where(u => u.id_estado == id_estado.Value);
            if (id_pais.HasValue) query = query.Where(u => u.id_pais == id_pais.Value);

            var vm = new UsuariosIndexVM
            {
                FiltroTexto = q,
                EstadoId = id_estado,
                PaisId = id_pais,
                Items = await query
                    .OrderBy(u => u.nombre).ThenBy(u => u.apellido)
                    .ToListAsync(),
                Estados = new SelectList(await _context.Estado
                    .OrderBy(e => e.nombre).ToListAsync(), "id_estado", "nombre", id_estado),
                Paises = new SelectList(await _context.Pais
                    .OrderBy(p => p.nombre).ToListAsync(), "id_pais", "nombre", id_pais),
            };

            return View(vm);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewData["id_estado"] = new SelectList(_context.Estado, "id_estado", "codigo");
            ViewData["id_pais"] = new SelectList(_context.Pais, "id_pais", "nombre");
            ViewData["id_rol"] = new SelectList(_context.Rol, "id_rol", "nombre");

            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_usuario,nombre,correo,contrasena_hash,fecha_creacion,fecha_actualizacion,id_pais,id_estado,apellido,id_rol")]
        Usuario usuario)
        {
            if (!String.IsNullOrEmpty(usuario.nombre) && !String.IsNullOrEmpty(usuario.contrasena_hash) && !String.IsNullOrEmpty(usuario.correo))
            {
                usuario.id_estado = (int)EstadoCodigo.Activa;
                usuario.fecha_creacion = DateTime.UtcNow;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_estado"] = new SelectList(_context.Estado, "id_estado", "codigo", usuario.id_estado);
            ViewData["id_pais"] = new SelectList(_context.Pais, "id_pais", "nombre", usuario.id_pais);
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["id_estado"] = new SelectList(_context.Estado, "id_estado", "codigo", usuario.id_estado);
            ViewData["id_pais"] = new SelectList(_context.Pais, "id_pais", "nombre", usuario.id_pais);
            ViewData["id_rol"] = new SelectList(_context.Rol, "id_rol", "nombre", usuario.id_rol);

            return View(usuario);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["id_estado"] = new SelectList(_context.Estado, "id_estado", "codigo", usuario.id_estado);
            ViewData["id_pais"] = new SelectList(_context.Pais, "id_pais", "nombre", usuario.id_pais);
            ViewData["id_rol"] = new SelectList(_context.Rol, "id_rol", "nombre", usuario.id_rol);

            return View(usuario);
        }
        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_usuario,nombre,correo,contrasena_hash,fecha_creacion,fecha_actualizacion,id_pais,id_estado,apellido,id_rol")]
        Usuario usuario)
        {
            if (id != usuario.id_usuario)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(usuario.nombre))
            {
                try
                {
                    //var infoUsuario = await _context.Usuario.FindAsync(usuario.id_usuario);
                    //usuario.contrasena_hash = infoUsuario.contrasena_hash;
                    usuario.fecha_actualizacion = DateTime.UtcNow;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.id_usuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_estado"] = new SelectList(_context.Estado, "id_estado", "codigo", usuario.id_estado);
            ViewData["id_pais"] = new SelectList(_context.Pais, "id_pais", "nombre", usuario.id_pais);
            ViewData["id_rol"] = new SelectList(_context.Rol, "id_rol", "nombre", usuario.id_rol);

            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.id_estadoNavigation)
                .Include(u => u.id_paisNavigation)
                .FirstOrDefaultAsync(m => m.id_usuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
        
        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.id_usuario == id);
        }
    }
}

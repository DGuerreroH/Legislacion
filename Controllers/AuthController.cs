using LegislacionAPP.Data;
using LegislacionAPP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LegislacionAPP.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null) => View(new LoginVM { ReturnUrl = returnUrl });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (String.IsNullOrEmpty(vm.Email) || String.IsNullOrEmpty(vm.Password)) return View(vm);

            var user = await _context.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.correo == vm.Email && u.id_estado == /*Activa*/ 2);

            if (user == null || (vm.Password != user.contrasena_hash))
            {
                ModelState.AddModelError("", "Credenciales inválidas.");
                return View(vm);
            }

            // claims básicos
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.id_usuario.ToString()),
            new Claim(ClaimTypes.Name, $"{user.nombre} {user.apellido}".Trim()),
            new Claim(ClaimTypes.Email, user.correo)
        };

            // Agregar roles para políticas globales.
            var roles = await _context.Usuario
                .Where(ue => ue.id_usuario == user.id_usuario)
                .Select(ue => ue.id_rolNavigation.nombre)
                .Distinct()
                .ToListAsync();

            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));
          

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Denied() => View();

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.id_estadoNavigation)
                .Include(u => u.id_paisNavigation)
                .Include(u => u.id_rolNavigation)
                .FirstOrDefaultAsync(m => m.id_usuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
    }
}

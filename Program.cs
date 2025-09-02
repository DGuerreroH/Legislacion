using LegislacionAPP.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Auth/Login";   // o /Usuarios/Login si usas ese controlador
        o.AccessDeniedPath = "/Auth/Denied";
        o.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

//// Políticas por rol
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(o =>
//    {
//        o.LoginPath = "/Auth/Login";
//        o.AccessDeniedPath = "/Auth/Denied";
//        o.SlidingExpiration = true;

//        // Anti-bucle: si ya estás en /Auth/Login, no vuelvas a redirigir
//        o.Events.OnRedirectToLogin = ctx =>
//        {
//            if (ctx.Request.Path.StartsWithSegments("/Auth/Login",
//                    StringComparison.OrdinalIgnoreCase))
//            {
//                // no redirijas otra vez; deja que muestre la vista de login
//                return Task.CompletedTask;
//            }
//            ctx.Response.Redirect(ctx.RedirectUri);
//            return Task.CompletedTask;
//        };
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

app.UseRouting();

app.UseAuthentication();   // <— IMPORTANTE antes de Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

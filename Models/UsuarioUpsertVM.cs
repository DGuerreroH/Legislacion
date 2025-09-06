using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegislacionAPP.Models
{
    public class UsuarioUpsertVM
    {
        public int? id_usuario { get; set; } // null = crear, valor = editar

        // Datos de usuario
        public string nombre { get; set; } = "";
        public string apellido { get; set; } = "";
        public string correo { get; set; } = "";
        public string? password { get; set; }
        public int id_rol { get; set; }
        public int id_estado { get; set; } = 2;
        public int id_pais { get; set; } = 1;

        // Asignaciones por empresa
        public List<UsuarioEmpresaAsignacionVM> Asignaciones { get; set; } = new();

        // Catálogos para la vista
        public IEnumerable<SelectListItem> Paises { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Empresas { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Roles { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

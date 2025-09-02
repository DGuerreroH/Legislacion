using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LegislacionAPP.Models
{
    public class EmpresaFormVM
    {
        public int id_empresa { get; set; }

        [Required, StringLength(200)]
        public string nombre { get; set; } = "";
        public string? representante { get; set; } = "";
        public string? nit { get; set; } = "";

        public string? logo { get; set; }              // ruta pública actual (si ya tiene)
        [Display(Name = "Logo")] public IFormFile? LogoFile { get; set; }  // archivo nuevo

        [Required] public int id_pais { get; set; }
        [Required] public int id_sector { get; set; }
        public int id_estado { get; set; }             // lo forzamos en Create y lo mantenemos en Edit

        public DateTime fecha_creacion { get; set; }

        // Selects
        public SelectList? Paises { get; set; }
        public SelectList? Sectores { get; set; }
        public SelectList? Estados { get; set; }
    }
}

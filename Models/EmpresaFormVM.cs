using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LegislacionAPP.Models
{
    public class EmpresaFormVM
    {
        public int id_empresa { get; set; }
        public int id_estado { get; set; }
        public string? logo { get; set; }
        public string nombre { get; set; } = "";
        public string? representante { get; set; }
        public string? nit { get; set; }
        public int id_pais { get; set; }
        public int id_sector { get; set; }
        public int id_ambito { get; set; }

        public DateTime fecha_creacion { get; set; }

        // UI
        public bool IsNew => id_empresa == 0;
        [Display(Name = "Logo")] public IFormFile? LogoFile { get; set; }  // archivo nuevo


        // Selects
        public IEnumerable<SelectListItem> Paises { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Sectores { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Ambito { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Estados { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegislacionAPP.Models
{
    public class LegislacionCreateVM
    {
        public int id_empresa { get; set; }
        public string EmpresaNombre { get; set; } = "";

        public int id_ambito_aplicacion { get; set; }
        public IEnumerable<SelectListItem> Ambitos { get; set; } = new List<SelectListItem>();

        public string? titulo { get; set; }
        public string? subtitulo { get; set; }
        public string? alias { get; set; }
        public string? codigo_interno { get; set; }
        public DateTime? fecha_vigencia { get; set; }
        public DateTime? fecha_creacion { get; set; }
        public int id_pais { get; set; }

        public SelectList? Paises { get; set; }
        // Archivo a subir
        public IFormFile? Pdf { get; set; }

        // Por si re-renderizamos o ya quedó guardado
        public string? pdf_url { get; set; }
    }
}

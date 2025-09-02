using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegislacionAPP.Models
{
    public class UsuariosIndexVM
    {
        public string? FiltroTexto { get; set; }
        public int? EstadoId { get; set; }
        public int? PaisId { get; set; }

        public IEnumerable<Usuario> Items { get; set; } = Enumerable.Empty<Usuario>();

        public SelectList? Estados { get; set; }
        public SelectList? Paises { get; set; }
    }
}

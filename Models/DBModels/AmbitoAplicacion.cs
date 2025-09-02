using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegislacionAPP.Models.DBModels
{
    public class AmbitoAplicacion
    {
        [Key, Column("id_ambito_aplicacion")] public int id_ambito_aplicacion { get; set; }

        [Required, Column("nombre"), StringLength(50)]
        public string nombre { get; set; } = null!;

        [Column("descripcion"), StringLength(200)]
        public string? descripcion { get; set; }

        public List<Legislacion> Legislaciones { get; set; } = new();

    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegislacionAPP.Models.DBModels
{
    [Table("CCMICategoria")]
    [Index(nameof(nombre), IsUnique = true)]
    public class CCMICategoria
    {
        [Key, Column("id_ccmi_categoria")]
        public int id_ccmi_categoria { get; set; }

        [Column("nombre"), StringLength(120)]
        public string nombre { get; set; } = null!;

        [Column("codigo"), StringLength(120)]
        public string codigo { get; set; } = null!;

        [Column("peso_categoria"), Precision(5, 2)]
        public decimal peso_categoria { get; set; } = 1m;

        [Column("orden")]
        public int orden { get; set; } = 1;

        [Column("activo")]
        public bool activo { get; set; } = true;

        [Column("fecha_creacion")]
        public DateTime fecha_creacion { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime? fecha_actualizacion { get; set; }

        [InverseProperty(nameof(CMMIPregunta.Categoria))]
        public ICollection<CMMIPregunta> Preguntas { get; set; } = new List<CMMIPregunta>();
    }
}

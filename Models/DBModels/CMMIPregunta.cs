using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegislacionAPP.Models.DBModels
{
    [Table("CMMIPregunta")]
    // Nota: si quisieras un índice único por (id_ccmi_categoria, codigo) ignorando NULL,
    // eso requiere un índice filtrado y se hace con migración/Fluent API.
    public class CMMIPregunta
    {
        [Key, Column("id_cmmi_pregunta")]
        public int id_cmmi_pregunta { get; set; }

        [Column("id_ccmi_categoria")]
        public int id_ccmi_categoria { get; set; }

        [Column("codigo"), StringLength(32)]
        public string? codigo { get; set; }

        [Column("texto"), StringLength(1000)]
        public string texto { get; set; } = null!;

        // DECIMAL(5,2)
        [Column("peso_pregunta"), Precision(5, 2)]
        public decimal peso_pregunta { get; set; } = 1m;

        [Column("es_critica")]
        public bool es_critica { get; set; } = false;

        [Column("orden")]
        public int orden { get; set; } = 1;

        [Column("activo")]
        public bool activo { get; set; } = true;

        [Column("fecha_creacion")]
        public DateTime fecha_creacion { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime? fecha_actualizacion { get; set; }

        [ForeignKey(nameof(id_ccmi_categoria))]
        [InverseProperty(nameof(CCMICategoria.Preguntas))]
        public CCMICategoria Categoria { get; set; } = null!;
        [ForeignKey(nameof(id_ccmi_categoria))] public CMMIEvaluacion? id_ccmi_categoriaNavigation { get; set; } = default!;

    }
}

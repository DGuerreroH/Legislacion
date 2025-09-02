using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegislacionAPP.Models.DBModels
{
    [Table("CMMIRespuesta")]
    public class CMMIRespuesta
    {
        [Key, Column("id_respuesta")]
        public int id_respuesta { get; set; }

        [Column("id_evaluacion")] public int id_evaluacion { get; set; }
        [Column("id_ccmi_pregunta")] public int id_ccmi_pregunta { get; set; }

        [Column("valor")] public byte valor { get; set; } // 0..5

        [Column("nota"), StringLength(1000)] public string? observacion { get; set; }
        [Column("evidencia_url"), StringLength(500)] public string? evidencia_url { get; set; }
        [Column("fecha_creacion")] public DateTime fecha_creacion { get; set; } = DateTime.UtcNow;


        [ForeignKey(nameof(id_evaluacion))] public CMMIEvaluacion? id_evaluacionNavigation { get; set; } = default!;
        [ForeignKey(nameof(id_ccmi_pregunta))] public CMMIPregunta? id_preguntaNavigation { get; set; } = default!;
    }
}

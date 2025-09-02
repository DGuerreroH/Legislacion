namespace LegislacionAPP.Models.DBModels;
// Models/EvaluacionSegmento.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;


[Table("EvaluacionSegmento")]
public class EvaluacionSegmento
{
    [Key, Column("id_evaluacion_segmento")] public int id_evaluacion_segmento { get; set; }

    [Column("id_ciclo_auditoria")] public int id_ciclo_auditoria { get; set; }
    [Column("id_segmento_legislacion")] public int id_segmento_legislacion { get; set; }
    [Column("id_usuario_auditor")] public int id_auditor { get; set; }
    [Column("id_estado")] public int id_estado { get; set; }

    [Column("comentario"), StringLength(1000)] public string? comentario { get; set; }
    [Column("fecha_evaluacion")] public DateTime fecha_evaluacion { get; set; }
    [Column("fecha_actualizacion")] public DateTime fecha_actualizacion { get; set; }


    [ForeignKey(nameof(id_ciclo_auditoria))] public CicloAuditoria id_ciclo_auditoriaNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_segmento_legislacion))] public SegmentoLegislacion id_segmento_legislacionNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_auditor))] public Usuario id_auditorNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_estado))] public Estado id_estadoNavigation { get; set; } = null!;

    public List<Evidencia> Evidencias { get; set; } = new();
}

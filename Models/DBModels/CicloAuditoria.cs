namespace LegislacionAPP.Models.DBModels;
// Models/CicloAuditoria.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CicloAuditoria")]
public class CicloAuditoria
{
    [Key, Column("id_ciclo_auditoria")] public int id_ciclo_auditoria { get; set; }

    [Column("id_legislacion")] public int id_legislacion { get; set; }
    [Column("id_estado")] public int id_estado { get; set; }
    [Column("id_usuario_aprobador")] public int id_usuario { get; set; }


    [Column("fecha_inicio")] public DateTime fecha_inicio { get; set; }
    [Column("fecha_cierre")] public DateTime? fecha_cierre { get; set; }

    [Column("total_segmentos")] public int articulos_totales { get; set; }
    [Column("total_aprobados")] public int articulos_aprobados { get; set; }
    [Column("porcentaje_aprobado")] public decimal? porcentaje_aprobado { get; set; } // 0..100
    [Column("motivo_cierre"), StringLength(1000)] public string? motivo_cierre { get; set; }
    [Column("resumen"), StringLength(1000)] public string? resumen { get; set; }


    [ForeignKey(nameof(id_usuario))] public Usuario id_usuarioNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_legislacion))] public Legislacion id_legislacionNavigation { get; set; } = null!;

    //[ForeignKey(nameof(id_auditor))] public Usuario id_auditorNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_estado))] public Estado id_estadoNavigation { get; set; } = null!;
    //public Legislacion Legislacion { get; set; } = null!;

    public List<EvaluacionSegmento> Evaluaciones { get; set; } = [];
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool Certificable =>
        fecha_cierre != null &&
        ((porcentaje_aprobado ?? // si no hay snapshot, recalcúlalo
          (articulos_totales == 0 ? 0
            : Math.Round(100m * articulos_aprobados / (decimal)articulos_totales, 2)
          )) >= 100m);


}

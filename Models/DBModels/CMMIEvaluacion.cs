using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegislacionAPP.Models.DBModels
{
    [Table("CMMIEvaluacion")]
    public class CMMIEvaluacion
    {
        [Key, Column("id_evaluacion")]
        public int id_evaluacion { get; set; }

        [Column("id_empresa")] public int id_empresa { get; set; }
        [Column("id_usuario_auditor")] public int id_usuario_auditor { get; set; }
        [Column("fecha_inicio")] public DateTime fecha_inicio { get; set; } = DateTime.UtcNow;
        [Column("fecha_cierre")] public DateTime? fecha_cierre { get; set; }
        [Column("id_estado")] public int id_estado { get; set; }

        [Column("puntaje_global", TypeName = "decimal(6,2)")]
        public decimal? puntaje_global { get; set; }

        [Column("nivel_madurez")] public byte? nivel_madurez { get; set; }
        [Column("observaciones"), StringLength(1000)]
        public string? observaciones { get; set; }

        [Column("fecha_creacion")] public DateTime fecha_creacion { get; set; } = DateTime.UtcNow;
        [Column("fecha_actualizacion")] public DateTime? fecha_actualizacion { get; set; }

        // Navs opcionales
        [ForeignKey(nameof(id_empresa))] public Empresa? Empresa { get; set; }
        [ForeignKey(nameof(id_usuario_auditor))] public Usuario? id_UsuarioAuditorNavigation { get; set; }
        [ForeignKey(nameof(id_estado))] public Estado id_estadoNavigation { get; set; } = null!;
        public ICollection<CMMIRespuesta> Respuestas { get; set; } = new List<CMMIRespuesta>();
    }
}

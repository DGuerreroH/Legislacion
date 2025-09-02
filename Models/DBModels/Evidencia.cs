namespace LegislacionAPP.Models.DBModels;
// Models/Evidencia.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Evidencia")]
public class Evidencia
{
    [Key, Column("id_evidencia")] public int id_evidencia { get; set; }

    [Column("id_evaluacion_segmento")] public int id_evaluacion_segmento { get; set; }
    [Column("archivo_url"), StringLength(500)] public string? contenido_url { get; set; }

    [Column("nombre_original"), StringLength(255)] public string? nombre_archivo { get; set; }
    [Column("mime_type"), StringLength(100)] public string? mime_type { get; set; }
    [Column("tamano_bytes")] public int? tamano_bytes { get; set; }
    [Column("sha256"), StringLength(500)] public string? sha256 { get; set; }

    [Column("tipo_documento"), StringLength(100)] public string? tipo_documento { get; set; }

    [Column("comentario_opcional"), StringLength(400)] public string? comentario_opcional { get; set; }
    [Column("fecha_subida")] public DateTime fecha_subida { get; set; }
    [Column("fecha_actualizacion")] public DateTime? fecha_actualizacion { get; set; }
    [Column("id_usuario")] public int? id_usuario { get; set; }

    [ForeignKey(nameof(id_evaluacion_segmento))] public EvaluacionSegmento id_evaluacion_segmentoNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_usuario))] public Usuario? id_usuario_Navigation { get; set; }
}

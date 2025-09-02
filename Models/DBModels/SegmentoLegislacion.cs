namespace LegislacionAPP.Models.DBModels;

// Models/SegmentoLegislacion.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("SegmentoLegislacion")]
public class SegmentoLegislacion
{
    [Key, Column("id_segmento_legislacion")] public int id_segmento_legislacion { get; set; }

    [Column("id_legislacion")] public int id_legislacion { get; set; }
    [Column("id_tipo_elemento")] public int id_tipo_elemento { get; set; }
    [Column("id_segmento_padre")] public int? id_segmento_padre { get; set; }
    [Column("contenido"), StringLength(1000)] public string? contenido { get; set; }

    [Column("observaciones"), StringLength(1000)] public string? observaciones { get; set; }
    [Column("orden")] public int orden { get; set; }
    [Column("tituloSegmento"), StringLength(100)] public string? tituloSegmento { get; set; }


    // contenido opcional (imagen/doc)
    [Column("contenido_url"), StringLength(500)] public string? contenido_url { get; set; }
    [Column("contenido_bin")] public byte[]? contenido_bin { get; set; }
    [Column("fecha_creacion")] public DateTime fecha_creacion { get; set; }
    [Column("fecha_actualizacion")] public DateTime? fecha_actualizacion { get; set; }
    [ForeignKey(nameof(id_legislacion))] public Legislacion id_legislacionNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_tipo_elemento))] public TipoElemento id_tipo_elementoNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_segmento_padre))] public SegmentoLegislacion? id_segmento_padreNavigation { get; set; }

    [InverseProperty(nameof(id_segmento_padreNavigation))]
    public List<SegmentoLegislacion> Hijos { get; set; } = new();

    public List<EvaluacionSegmento> Evaluaciones { get; set; } = new();
}

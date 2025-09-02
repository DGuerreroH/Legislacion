namespace LegislacionAPP.Models.DBModels;
    
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Legislacion")]
public class Legislacion
{
    [Key, Column("id_legislacion")] public int id_legislacion { get; set; }

    [Column("id_empresa")] public int id_empresa { get; set; }
    [Column("id_estado")] public int id_estado { get; set; }
    [Column("id_ambito_aplicacion")] public int id_ambito_aplicacion { get; set; }
    [Column("id_usuario_creador")] public int id_usuario { get; set; }


    [Required, Column("titulo"), StringLength(200)] public string titulo { get; set; } = null!;
    [Column("subtitulo"), StringLength(200)] public string? subtitulo { get; set; }
    [Column("alias"), StringLength(80)] public string? alias { get; set; }
    [Column("codigo_interno"), StringLength(50)] public string? codigo_interno { get; set; }

    [Column("fecha_vigencia")] public DateTime? fecha_vigencia { get; set; }
    [Column("archivo_pdf_url"), StringLength(500)] public string? pdf_url { get; set; }
    [Column("fecha_creacion")] public DateTime fecha_creacion { get; set; }
    [Column("fecha_actualizacion")] public DateTime? fecha_actualizacion { get; set; }

    [Column("id_pais")] public int id_pais { get; set; }

    // FK
    [ForeignKey(nameof(id_pais))] public Pais id_paisNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_empresa))] public Empresa id_empresaNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_estado))] public Estado id_estadoNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_ambito_aplicacion))] public AmbitoAplicacion id_ambito_aplicacionNavigation { get; set; } = null!;

    public List<SegmentoLegislacion> Segmentos { get; set; } = new();
    public List<CicloAuditoria> Ciclos { get; set; } = new();
}

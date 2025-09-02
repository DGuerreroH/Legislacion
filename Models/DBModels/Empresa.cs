namespace LegislacionAPP.Models.DBModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Empresa")]
public class Empresa
{
    [Key, Column("id_empresa")] public int id_empresa { get; set; }
    
    [Display(Name = "Nombre empresa")]
    [Required, Column("nombre"), StringLength(150)] public string nombre { get; set; } = null!;
    [Column("representante"), StringLength(150)] public string? representante { get; set; } = null!;
    [Column("nit"), StringLength(150)] public string? nit { get; set; } = null!;

    [Column("logo"), StringLength(500)] public string? logo { get; set; }

    [Column("id_pais")] public int id_pais { get; set; }
    [Column("id_estado")] public int id_estado { get; set; }

    [Column("fecha_creacion")] public DateTime fecha_creacion { get; set; }
    [Column("fecha_actualizacion")] public DateTime? fecha_actualizacion { get; set; }
    // ...
    [Column("id_sector")]
    public int id_sector { get; set; }     
    
    // FK
    [ForeignKey(nameof(id_pais))] public Pais id_paisNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_estado))] public Estado id_estadoNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_sector))] public Sector id_sectorNavigation { get; set; } = null!; 

    public List<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new();
    public List<Legislacion> Legislacions { get; set; } = new();
}

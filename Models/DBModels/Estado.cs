namespace LegislacionAPP.Models.DBModels;
// Models/Estado.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Estado")]
public class Estado
{
    [Key, Column("id_estado")] public int id_estado { get; set; }

    [Required, Column("codigo"), StringLength(30)] public string codigo { get; set; } = null!;
    [Required, Column("nombre"), StringLength(80)] public string nombre { get; set; } = null!;
    [Column("descripcion"), StringLength(200)] public string? descripcion { get; set; }
    [Column("color_hex"), StringLength(7)] public string? color_hex { get; set; }

    public List<Empresa> Empresas { get; set; } = new();
    public List<Legislacion> Legislaciones { get; set; } = new();
    public List<CicloAuditoria> Ciclos { get; set; } = new();
    public List<EvaluacionSegmento> Evaluaciones { get; set; } = new();
}

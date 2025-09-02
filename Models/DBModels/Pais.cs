namespace LegislacionAPP.Models.DBModels;
// Models/Pais.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Pais")]
public class Pais
{
    [Key, Column("id_pais")] public int id_pais { get; set; }

    [Required, Column("nombre"), StringLength(100)]
    public string nombre { get; set; } = null!;

    [Column("codigo_iso"), StringLength(3)]
    public string? codigo_iso { get; set; }

    //Navegación(opcional)
    public List<Empresa> Empresas { get; set; } = new List<Empresa>();
}

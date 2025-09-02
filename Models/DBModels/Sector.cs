using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegislacionAPP.Models.DBModels;

[Table("Sector")]
public class Sector
{
    [Key, Column("id_sector")]
    public int id_sector { get; set; }

    [Required, Column("nombre"), StringLength(100)]
    public string nombre { get; set; } = null!;

    [Column("descripcion"), StringLength(200)]
    public string? descripcion { get; set; }

    // navegación inversa
    public List<Empresa> Empresas { get; set; } = new();
}

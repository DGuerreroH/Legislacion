namespace LegislacionAPP.Models.DBModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Rol")]
public class Rol
{
    [Key, Column("id_rol")] public int id_rol { get; set; }

    [Required, Column("nombre"), StringLength(40)]
    public string nombre { get; set; } = null!;

    [Column("descripcion"), StringLength(150)]
    public string? descripcion { get; set; }

    public List<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new();
}

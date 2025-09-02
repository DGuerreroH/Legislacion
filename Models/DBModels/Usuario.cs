namespace LegislacionAPP.Models.DBModels;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Usuario")]
public class Usuario
{
    [Key, Column("id_usuario")] public int id_usuario { get; set; }

    [Required, Column("nombre"), StringLength(100)] public string nombre { get; set; } = null!;
    [Required, Column("apellido"), StringLength(100)] public string apellido { get; set; } = null!;
    [Required, Column("correo"), StringLength(150)] public string correo { get; set; } = null!;
    [Required, Column("contrasena_hash"), StringLength(200)] public string contrasena_hash { get; set; } = null!;
    [Column("fecha_creacion")] public DateTime fecha_creacion { get; set; }
    [Column("fecha_actualizacion")] public DateTime? fecha_actualizacion { get; set; }
    [Column("id_pais")] public int id_pais { get; set; }
    [Column("id_estado")] public int id_estado { get; set; }
    [Column("id_rol")] public int id_rol { get; set; }

    // FK
    [ForeignKey(nameof(id_pais))] public Pais id_paisNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_estado))] public Estado id_estadoNavigation { get; set; } = null!;

    [ForeignKey(nameof(id_rol))] public Rol id_rolNavigation { get; set; } = null!;

    public List<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new();
}

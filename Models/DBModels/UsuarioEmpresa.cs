namespace LegislacionAPP.Models.DBModels;
// Models/UsuarioEmpresa.cs  (relación usuario-empresa-rol)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("UsuarioEmpresa")]
public class UsuarioEmpresa
{
    [Key, Column("id_usuario_empresa")] public int id_usuario_empresa { get; set; }

    [Column("id_usuario")] public int id_usuario { get; set; }
    [Column("id_empresa")] public int id_empresa { get; set; }
    [Column("id_rol")] public int id_rol { get; set; }

    [Column("fecha_asignacion")] public DateTime fecha_asignacion { get; set; }

    [ForeignKey(nameof(id_usuario))] public Usuario id_usuarioNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_empresa))] public Empresa id_empresaNavigation { get; set; } = null!;
    [ForeignKey(nameof(id_rol))] public Rol id_rolNavigation { get; set; } = null!;
}

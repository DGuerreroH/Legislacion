namespace LegislacionAPP.Models
{
    public class UsuarioEmpresaAsignacionVM
    {
        public int? id_usuario_empresa { get; set; }
        public int id_empresa { get; set; }
        public int id_rol { get; set; }
        public string? empresa_nombre { get; set; }
    }
}

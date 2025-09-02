namespace LegislacionAPP.Models
{
    public class EmpresaIndexRowVM
    {
        public int id_empresa { get; set; }
        public string nombre { get; set; } = "";
        public string? logo { get; set; }
        public string pais { get; set; } = "";
        public string sector { get; set; } = "";
        public string estado { get; set; } = "";       // ej.: "Activa", "Inactiva"
        public DateTime fecha_creacion { get; set; }
    }
}

namespace LegislacionAPP.Models
{
    public class CiclosPorLegislacionVM
    {
        public int id_legislacion { get; set; }
        public int id_empresa { get; set; }
        public string Empresa { get; set; } = "";
        public string Legislacion { get; set; } = "";
        public List<CicloAuditoriaRowVM> Ciclos { get; set; } = new();
    }
}

namespace LegislacionAPP.Models
{
    public class AuditoriaLegislacionVM
    {
        public int id_legislacion { get; set; }
        public int id_ciclo_auditoria { get; set; }

        public string? EmpresaNombre { get; set; }
        public string? Titulo { get; set; }

        public List<SegmentoAuditableVM> Segmentos { get; set; } = new();
    }
}

namespace LegislacionAPP.Models
{
    public class CicloAuditoriaRowVM
    {
        public int id_ciclo_auditoria { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public string Auditor { get; set; } = "";
        public int TotalSegmentos { get; set; }
        public int Aprobados { get; set; }
        public decimal Porcentaje { get; set; }  // 0..100
        public bool Abierto => fecha_cierre == null;
    }

}

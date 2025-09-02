namespace LegislacionAPP.Models
{
    public class CMMIEvalRowVM
    {
        public int id_evaluacion { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public string Auditor { get; set; } = "";
        public int id_estado { get; set; }
        public string Estado { get; set; } = "";
        public decimal Porcentaje { get; set; } // 0..100
        public int Nivel { get; set; }          // 1..5
    }
}

using LegislacionAPP.Common.Enums;

namespace LegislacionAPP.Models
{
    public class ReporteAuditoriaVM
    {
        public int id_ciclo_auditoria { get; set; }
        public string Empresa { get; set; } = "";
        public string Legislacion { get; set; } = "";
        public string Auditor { get; set; } = "";
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }

        public int TotalSegmentos { get; set; }
        public int Aprobados { get; set; }
        public int NoAprobados { get; set; }
        public int Pendientes { get; set; }
        public decimal AvancePorc { get; set; }   // 0..100

        public List<ReporteAuditoriaRow> Items { get; set; } = new();
    }

    public class ReporteAuditoriaRow
    {
        public int Orden { get; set; }
        public string TipoElemento { get; set; } = "";
        public string Contenido { get; set; } = "";       // corto / resumido
        public string? ComentarioAuditor { get; set; }
        public EstadoCodigo Estado { get; set; }
        public int Evidencias { get; set; }
    }
}

namespace LegislacionAPP.Models
{
    public class AuditoriaContinuarVM
    {
        // Identificadores
        public int id_legislacion { get; set; }
        public int id_ciclo_auditoria { get; set; }
        public bool randomSave { get; set; } = false;
        public string? accion { get; set; } 


        // Header
        public string? EmpresaNombre { get; set; }
        public string? Titulo { get; set; }
        public string? AuditorNombre { get; set; }

        // Ciclo
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public int articulos_totales { get; set; }
        public int articulos_aprobados { get; set; }
        public decimal porcentaje_avance { get; set; } // 0..100
        public bool CicloAbierto => fecha_cierre == null;

        // Segmentos a evaluar
        public List<SegmentoAuditableVM> Segmentos { get; set; } = new();
    }    

} 

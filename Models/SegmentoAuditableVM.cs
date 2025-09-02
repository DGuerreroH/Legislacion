using LegislacionAPP.Models.DBModels;

namespace LegislacionAPP.Models
{
    public class SegmentoAuditableVM
    {
        public int id_segmento_legislacion { get; set; }
        public int? id_evaluacion_segmento { get; set; }

        public string? contenido { get; set; }        // Solo para mostrar
        public bool? aprobado { get; set; }           // true/false/null
        public string? observaciones { get; set; }
        public string? segmentoTitulo { get; set; }
        public int id_tipo_elemento { get; set; }
        public string? tipo_elemento { get; set; }
        public int id_usuario_auditor { get; set; }
        public int? id_estado { get; set; }
        // Evidencias existentes
        public List<Evidencia> Evidencias { get; set; } = new();

    }
}

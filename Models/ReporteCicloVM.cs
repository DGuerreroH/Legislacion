using LegislacionAPP.Common.Enums;
using LegislacionAPP.Models.DBModels;

namespace LegislacionAPP.Models
{
    public class ReporteCicloVM
    {
        // Cabecera (objetos completos, como pediste)
        public Empresa Empresa { get; set; } = new();
        public Legislacion Legislacion { get; set; } = new();
        public Usuario Auditor { get; set; } = new();
        public CicloAuditoria CicloAuditoria { get; set; } = new();

        // Derivados limpios para la vista (evitan null-check en Razor)
        public string EmpresaNombre => Empresa?.nombre ?? "";
        public string EmpresaPais => Empresa?.id_paisNavigation?.nombre ?? "";
        public string AuditorNombre => $"{Auditor?.nombre} {Auditor?.apellido}".Trim();
        public string AuditorPais => Auditor?.id_paisNavigation?.nombre ?? "";
        public string TituloLegislacion => Legislacion?.titulo ?? "";

        // Métricas
        public int Total { get; set; }
        public int Cumple { get; set; }
        public int Parcial { get; set; }
        public int NoCumple { get; set; }
        public decimal Porcentaje { get; set; } // 0..100

        // Para saber si se renderiza para PDF
        public bool EsPdf { get; set; }

        public List<SegmentoAuditableVM> Articulos { get; set; } = new();
    }   
}
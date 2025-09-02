using LegislacionAPP.Models.DBModels;

namespace LegislacionAPP.Models
{
    public class CMMICertificadoVM
    {
        public Empresa Empresa { get; set; }
        public string? EmpresaPais { get; set; }
        public CMMIEvaluacion Evaluacion { get; set; }
        public Usuario Auditor { get; set; }

        public decimal PorcentajeGlobal { get; set; }       // 0..100
        public string NivelGlobal { get; set; } = "";
        public List<ItemCategoriaVM> Categorias { get; set; } = new();

        public string CertId { get; set; } = "";             // p.ej. CMMI-EMP1-EV10
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }      // opcional (p.ej. +12 meses)

        public string? LogoUrl { get; set; }                 // Empresa.logo
        public bool EsPdf { get; set; }                      // para simplificar estilos
    }
}

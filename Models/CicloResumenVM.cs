namespace LegislacionAPP.Models
{
    public class CicloResumenVM
    {
        public int id_ciclo_auditoria { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public int articulos_totales { get; set; }
        public int articulos_aprobados { get; set; }
        public decimal porcentaje_avance { get; set; }
        public string Estado => fecha_cierre == null ? "Abierto" : "Cerrado";
        public bool Certificable => porcentaje_avance == 100;

    }

    // ViewModels/AuditoriaCiclosVM.cs
    public class AuditoriaCiclosVM
    {
        public int id_legislacion { get; set; }
        public string? EmpresaNombre { get; set; }
        public int? EmpresaID { get; set; }
        public string? Titulo { get; set; }
        public List<CicloResumenVM> Ciclos { get; set; } = new();
        public bool TieneAbierto => Ciclos.Any(c => c.fecha_cierre == null);
    }
    public class CertificadoVM
    {
        public string Empresa { get; set; } = default!;
        public string? Pais { get; set; }
        public string Legislacion { get; set; } = default!;
        public DateTime FechaCierre { get; set; }
        public decimal? Porcentaje { get; set; }
        public string Folio { get; set; } = default!;
    }
    public class HistorialLegislacionVM
    {
        public string Empresa { get; set; } = default!;
        public string Legislacion { get; set; } = default!;
        public string? Ambito { get; set; }
        public List<CicloResumenVM> Ciclos { get; set; } = new();
    }
}

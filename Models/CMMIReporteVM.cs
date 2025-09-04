namespace LegislacionAPP.Models
{
    // ViewModels/CMMIReporteVM.cs
    public class CMMIReporteVM
    {
        // Cabecera
        public int id_empresa { get; set; }
        public int id_evaluacion { get; set; }
        public string Empresa { get; set; } = "";
        public string RepresentanteEmpresa { get; set; } = "";

        public string Auditor { get; set; } = "";
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }

        // Métricas
        public decimal PorcentajeGlobal { get; set; }
        public string NivelGlobal { get; set; } = "";
        public bool EsPdf { get; set; } = false;
        

        // Desglose por categoría
        public List<CategoriaVM> Categorias { get; set; } = new();
        public class CategoriaVM
        {
            public string Codigo { get; set; } = "";
            public string Nombre { get; set; } = "";
            public decimal Porcentaje { get; set; }   // 0..100
            public List<PreguntaVM> Preguntas { get; set; } = new();
        }

        public class PreguntaVM
        {
            public string Codigo { get; set; } = "";
            public string Texto { get; set; } = "";
            public int Valor { get; set; }             // valor crudo (0..5 ó 0..100)
            public decimal ValorPct { get; set; }      // 0..100 (normalizado)
            public bool EsCritica { get; set; }
            public string? Comentario { get; set; }
        }
    }

}

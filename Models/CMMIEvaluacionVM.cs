namespace LegislacionAPP.Models
{
    public class CMMIEvaluacionVM
    {
        public int id_evaluacion { get; set; }
        public int id_empresa { get; set; }
        public string EmpresaNombre { get; set; } = "";
        public List<CategoriaVM> Categorias { get; set; } = new();

        public class CategoriaVM
        {
            public int id_ccmi_categoria { get; set; }
            public string Nombre { get; set; } = "";
            public List<PreguntaVM> Preguntas { get; set; } = new();
        }

        public class PreguntaVM
        {
            public int Index { get; set; }                  // <- importante para el binding de la lista
            public int id_ccmi_pregunta { get; set; }
            public string texto { get; set; } = "";
            public byte? valor { get; set; }                // 0..5 (null si no respondida)
        }
    }
}

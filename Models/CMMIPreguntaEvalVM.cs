namespace LegislacionAPP.Models
{
    // VM por pregunta (lo que pintas en la vista)
    public class CMMIPreguntaEvalVM
    {
        public int id_pregunta { get; set; }
        public string codigo { get; set; } = "";
        public string texto { get; set; } = "";
        public decimal peso { get; set; }     // 1.00, 0.75, etc (de la tabla)
        public bool es_critica { get; set; }

        // Respuesta seleccionada: 0, 25, 50, 75, 100
        public int? valor { get; set; }

        // (Opcional) comentario por pregunta
        public string? comentario { get; set; }
    }

    public class CMMICategoriaEvalVM
    {
        public int id_categoria { get; set; }
        public string nombre { get; set; } = "";
        public int orden { get; set; }
        public List<CMMIPreguntaEvalVM> Preguntas { get; set; } = new();
    }

    public class CMMIEvaluarVM
    {
        public int id_empresa { get; set; }
        public int id_evaluacion { get; set; }
        public string Empresa { get; set; } = "";
        public string? Auditor { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public bool Cerrada => fecha_cierre != null;

        public List<CMMICategoriaEvalVM> Categorias { get; set; } = new();

        // Resultados calculados (para mostrar arriba mientras editas)
        public decimal PorcentajeGlobal { get; set; }
        public int NivelGlobal { get; set; }
    }

}

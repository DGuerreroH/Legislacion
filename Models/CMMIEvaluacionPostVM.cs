namespace LegislacionAPP.Models
{
    public class CMMIEvaluacionPostVM
    {
        public int id_evaluacion { get; set; }
        public int id_empresa { get; set; }

        // Respuestas posteadas: preguntaId -> valor (0..5)
        public List<RespuestaVM> Respuestas { get; set; } = new();

        public class RespuestaVM
        {
            public int id_ccmi_pregunta { get; set; }
            public byte valor { get; set; }      // 0..5
            public string? nota { get; set; }
        }
    }

}

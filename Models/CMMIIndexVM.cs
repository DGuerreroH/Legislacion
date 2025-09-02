namespace LegislacionAPP.Models
{
    public class CMMIIndexVM
    {
        public int id_empresa { get; set; }
        public string Empresa { get; set; } = "";
        public List<CMMIEvalRowVM> Evaluaciones { get; set; } = new();
    } 

}

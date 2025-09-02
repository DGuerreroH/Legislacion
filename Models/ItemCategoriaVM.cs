namespace LegislacionAPP.Models
{
    public class ItemCategoriaVM
    {
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public decimal Porcentaje { get; set; }              // 0..100
        public decimal TotalCalificacion { get; set; }              // 0..100

    }
}

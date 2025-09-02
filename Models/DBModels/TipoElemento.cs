namespace LegislacionAPP.Models.DBModels;

// Models/TipoElemento.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("TipoElemento")]
public class TipoElemento
{
    [Key, Column("id_tipo_elemento")] public int id_tipo_elemento { get; set; }

    [Required, Column("nombre"), StringLength(50)] public string nombre { get; set; } = null!;
    [Column("descripcion"), StringLength(150)] public string? descripcion { get; set; }

    public List<SegmentoLegislacion> Segmentos { get; set; } = new();
}

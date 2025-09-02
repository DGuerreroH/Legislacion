namespace LegislacionAPP.ViewModels;

public class LegislacionRowVM
{
    public int id_legislacion { get; set; }
    public string titulo { get; set; } = null!;
    public string? codigo_interno { get; set; }
    public string estado { get; set; } = "";
    public string? color_hex { get; set; }
    public int? total_segmentos { get; set; }
    public DateTime fecha_creacion { get; set; }
    public DateTime? fecha_vigencia { get; set; } // si tienes “fecha_entrada_vigencia”
    public bool certificable { get; set; } // si tiene ciclo cerrado y aprobado
}

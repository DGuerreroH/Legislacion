namespace LegislacionAPP.ViewModels;

public class EmpresaCardVM
{
    public int id_empresa { get; set; }
    public string nombre { get; set; } = null!;
    public string? logo { get; set; }
    public string pais { get; set; } = "";
    public string estado { get; set; } = "";
    public string rolAsignado { get; set; } = "";
}

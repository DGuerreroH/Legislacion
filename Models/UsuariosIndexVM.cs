using LegislacionAPP.Models.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegislacionAPP.Models
{
    public class UsuariosIndexVM
    {
        public int id_usuario { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Correo { get; set; } = "";
        public int IdEstado { get; set; }        // 2 = Activa (según tus seeds)
        public string? EstadoNombre { get; set; } // opcional
        public string RolNombre { get; set; } = "";
        public string? EmpresasLista { get; set; } // "Tigo2, Claro, Patito" (para badges)
    }
}

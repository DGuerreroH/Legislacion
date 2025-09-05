namespace LegislacionAPP.Models.Helpers
{
    public class Madurez
    {
        // Umbrales únicos para toda la app (ajusta si cambia tu política)
        public static int Nivel(decimal pct)
        {
            if (pct >= 90) return 5;
            if (pct >= 80) return 4;
            if (pct >= 60) return 3;
            if (pct >= 40) return 2;
            if (pct > 0) return 1;
            return 0;
        }

        public static string NombreNivel(int nivel) => nivel switch
        {
            5 => "Optimizado",
            4 => "Gestionado cuantitativamente",
            3 => "Definido",
            2 => "Gestionado",
            1 => "Inicial",
            _ => "Incompleto"
        };

    }
}

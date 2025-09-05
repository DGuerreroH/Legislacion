using LegislacionAPP.Models;

namespace LegislacionAPP.Common
{
    public static class LegislationEngine
    {
        private const int EST_CUMPLE = 6;
        private const int EST_PARCIAL = 10;
        private const int EST_NOCUMPLE = 7;

        public static decimal Score01(int? estadoId) => estadoId switch
        {
            EST_CUMPLE => 1m,
            EST_PARCIAL => 0.5m,
            EST_NOCUMPLE => 0m,
            _ => -1m
        };

        public static (decimal pct, decimal avance) Compute(IEnumerable<SegmentoAuditableVM> segs)
        {
            var eval = 0; var total = 0; decimal sum = 0m;
            foreach (var s in segs ?? Enumerable.Empty<SegmentoAuditableVM>())
            {
                total++;
                var v = Score01(s.id_estado);
                if (v < 0) continue;    // pendiente
                eval++;
                sum += v;
            }

            var pct = eval == 0 ? 0m : Math.Round((sum / eval) * 100m, 2);
            var avance = total == 0 ? 0m : Math.Round((eval * 100m) / total, 2);
            return (pct, avance);
        }
    }
}

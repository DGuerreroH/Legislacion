using System.ComponentModel.DataAnnotations;

namespace LegislacionAPP.Common.Enums;

public enum EstadoCodigo
{
    [Display(Name = "Cancelado", Description = "Ciclo invalidado por cambios")]
    [HexColor("#6c757d")]
    Cancelado = 1,

    [Display(Name = "Activa", Description = "Entidad activa")]
    [HexColor("#28a745")]
    Activa = 2,

    [Display(Name = "Inactiva", Description = "Entidad inactiva")]
    [HexColor("#6c757d")]
    Inactiva = 3,

    [Display(Name = "Borrador", Description = "En edición")]
    [HexColor("#ffc107")]
    Borrador = 4,

    [Display(Name = "En revisión", Description = "Ciclo en curso")]
    [HexColor("#007bff")]
    EnRevision = 5,

    [Display(Name = "Cumple", Description = "Cumple")]
    [HexColor("#28a745")]
    Cumple = 6,

    [Display(Name = "NoCumple", Description = "No cumple")]
    [HexColor("#dc3545")]
    NoCumple = 7,


    [Display(Name = "Archivada", Description = "No operativa")]
    [HexColor("#6c757d")]
    Archivada = 8,

    [Display(Name = "Observado", Description = "Requiere corrección")]
    [HexColor("#ffc107")]
    Observado = 9,

    [Display(Name = "Parcial", Description = "Cumple parcialmente")]
    [HexColor("#ffc107")]
    Parcial = 10,
}

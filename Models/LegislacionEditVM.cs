using Microsoft.AspNetCore.Mvc.Rendering;

namespace LegislacionAPP.ViewModels;

public class LegislacionEditVM
{
    // Legislación (cabecera)
    public int id_legislacion { get; set; }
    public int id_empresa { get; set; }
    public int id_estado { get; set; }
    public int id_ambito_aplicacion { get; set; }

    public string titulo { get; set; } = "";
    public string? subtitulo { get; set; }
    public string? alias { get; set; }
    public string? codigo_interno { get; set; }
    public DateTime fecha_creacion { get; set; }
    public DateTime? fecha_vigencia { get; set; }
    public string? pdf_url { get; set; }
    public IFormFile? Pdf { get; set; }
    // NUEVO → id del tipo "Foto" para usar en vista/js
    public int IdTipoElementoFoto { get; set; }
    public int id_pais { get; set; }

    // Hijos
    public List<SegmentoVM> Segmentos { get; set; } = new();

    // Combos
    public SelectList? Estados { get; set; }
    public SelectList? Empresas { get; set; }
    public SelectList? Ambitos { get; set; }
    public SelectList? Paises { get; set; }

    public SelectList? TiposElemento { get; set; }   // para cada card
}

public class SegmentoVM
{
    public int? id_segmento_legislacion { get; set; }   // null => nuevo
    public int id_legislacion { get; set; }             // se setea en el POST
    public int id_tipo_elemento { get; set; }

    public string? segmentoTitulo { get; set; }              // texto libre
    public string? contenido { get; set; }              // texto libre
    public string? observaciones { get; set; }
    public int orden { get; set; }                      // para ordenar cards

    // Soporte archivo opcional (imágenes/PDF embebidos)
    public IFormFile? archivo { get; set; }
    public string? archivo_url { get; set; }            // si ya existe
    public bool Eliminar { get; set; }                  // marcar para eliminar
   
}

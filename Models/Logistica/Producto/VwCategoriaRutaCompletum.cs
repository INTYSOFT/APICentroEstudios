using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;


[Table("vw_categoria_ruta_completa", Schema = "public")]
public partial class VwCategoriaRutaCompletum
{
    [Key]
    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [Column("categoria_main_id")]
    public int? CategoriaMainId { get; set; }

    [Column("categoria")]
    [StringLength(256)]
    public string? Categoria { get; set; }

    [Column("ruta")]
    public string? Ruta { get; set; }

    


}

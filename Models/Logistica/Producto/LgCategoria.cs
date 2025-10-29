using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_categoria")]
public partial class LgCategoria
{
    [Key]
    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    [Column("categoria_main_id")]
    public int? CategoriaMainId { get; set; }

    [Column("is_end")]
    public bool? IsEnd { get; set; }

    [Column("categoria")]
    [StringLength(32)]
    public string Categoria { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(128)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    //agrega campo is_editado_ficha_tecnicas
    [Column("is_editado_ficha_tecnica")] //Si es true indica que tienen su propia ficha tecnica.
    public bool? IsEditadoFichaTecnica { get; set; }


    //categoria_id_ficha_tecnica
    [Column("categoria_id_ficha_tecnica")]
    public int? CategoriaIdFichaTecnica { get; set; }


    [InverseProperty("Categoria")]
    public virtual ICollection<LgCategoriaFichaTecnica> LgCategoriaFichaTecnicas { get; set; } = new List<LgCategoriaFichaTecnica>();

    [InverseProperty("Categoria")]
    public virtual ICollection<LgProducto> LgProductos { get; set; } = new List<LgProducto>();
}

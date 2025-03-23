using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_atributo_producto_detalle")]
public partial class LgAtributoProductoDetalle
{
    [Key]
    [Column("atributo_producto_detalle_id")]
    public int AtributoProductoDetalleId { get; set; }

    [Column("atributo_producto_id")]
    public int? AtributoProductoId { get; set; }

    [Column("codigo_custom")]
    [StringLength(8)]
    public string? CodigoCustom { get; set; }

    [Column("valor")]
    [StringLength(32)]
    public string? Valor { get; set; }

    [Column("aliass")]
    [StringLength(8)]
    public string? Aliass { get; set; }

    [Column("is_default")]
    public bool? IsDefault { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("AtributoProductoId")]
    [InverseProperty("LgAtributoProductoDetalles")]
    public virtual LgAtributoProducto? AtributoProducto { get; set; }

    [InverseProperty("AtributoProductoDetalle")]
    public virtual ICollection<LgAtributoAsignadoProducto> LgAtributoAsignadoProductos { get; set; } = new List<LgAtributoAsignadoProducto>();
}

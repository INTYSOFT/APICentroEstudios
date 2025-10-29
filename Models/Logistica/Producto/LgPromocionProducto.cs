using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[PrimaryKey("ProductoId", "VarianteProductoId", "PromocionId")]
[Table("lg_promocion_producto")]
public partial class LgPromocionProducto
{
    [Key]
    [Column("promocion_id")]
    public int PromocionId { get; set; }

    [Key]
    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Key]
    [Column("variante_producto_id")]
    public int VarianteProductoId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("LgPromocionProductos")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("PromocionId")]
    [InverseProperty("LgPromocionProductos")]
    public virtual LgPromocion Promocion { get; set; } = null!;

    [ForeignKey("VarianteProductoId")]
    [InverseProperty("LgPromocionProductos")]
    public virtual LgProductoVariante VarianteProducto { get; set; } = null!;
}

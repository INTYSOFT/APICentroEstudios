using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_item_orden_compra")]
public partial class LgItemOrdenCompra
{
    [Key]
    [Column("item_orden_compra_id")]
    public int ItemOrdenCompraId { get; set; }

    [Column("orden_compra_id")]
    public int OrdenCompraId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("variante_producto_id")]
    public int? VarianteProductoId { get; set; }

    [Column("inventario_id")]
    public int? InventarioId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("precio_unidad")]
    [Precision(10, 2)]
    public decimal PrecioUnidad { get; set; }

    [Column("total_precio")]
    [Precision(10, 2)]
    public decimal? TotalPrecio { get; set; }

    [Column("estado")]
    public bool? Estado { get; set; }

    [ForeignKey("InventarioId")]
    [InverseProperty("LgItemOrdenCompras")]
    public virtual LgInventario? Inventario { get; set; }

    [ForeignKey("OrdenCompraId")]
    [InverseProperty("LgItemOrdenCompras")]
    public virtual LgOrdenCompra OrdenCompra { get; set; } = null!;

    [ForeignKey("ProductoId")]
    [InverseProperty("LgItemOrdenCompras")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("VarianteProductoId")]
    [InverseProperty("LgItemOrdenCompras")]
    public virtual LgProductoVariante? VarianteProducto { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_precio")]
public partial class LgPrecio
{
    [Key]
    [Column("precio_id")]
    public int PrecioId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("variante_producto_id")]
    public int? VarianteProductoId { get; set; }

    [Column("precio_compra")]
    [Precision(10, 2)]
    public decimal PrecioCompra { get; set; }

    [Column("precio_venta")]
    [Precision(10, 2)]
    public decimal PrecioVenta { get; set; }

    [Column("fecha_inicio", TypeName = "timestamp without time zone")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin", TypeName = "timestamp without time zone")]
    public DateTime? FechaFin { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("LgPrecios")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("VarianteProductoId")]
    [InverseProperty("LgPrecios")]
    public virtual LgProductoVariante? VarianteProducto { get; set; }
}

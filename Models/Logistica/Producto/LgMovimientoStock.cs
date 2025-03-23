using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_movimiento_stock")]
public partial class LgMovimientoStock
{
    [Key]
    [Column("movimiento_id")]
    public int MovimientoId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("variante_producto_id")]
    public int? VarianteProductoId { get; set; }

    [Column("almacen_id")]
    public int AlmacenId { get; set; }

    [Column("tipo_movimiento_id")]
    public int TipoMovimientoId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("fecha_movimiento", TypeName = "timestamp without time zone")]
    public DateTime FechaMovimiento { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("AlmacenId")]
    [InverseProperty("LgMovimientoStocks")]
    public virtual LgAlmacen Almacen { get; set; } = null!;

    [ForeignKey("ProductoId")]
    [InverseProperty("LgMovimientoStocks")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("TipoMovimientoId")]
    [InverseProperty("LgMovimientoStocks")]
    public virtual LgTipoMovimientoStock TipoMovimiento { get; set; } = null!;

    [ForeignKey("VarianteProductoId")]
    [InverseProperty("LgMovimientoStocks")]
    public virtual LgProductoVariante? VarianteProducto { get; set; }
}

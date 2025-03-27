using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_inventario")]
public partial class LgInventario
{
    [Key]
    [Column("inventario_id")]
    public int InventarioId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("producto_variante_id")]
    public int? ProductoVarianteId { get; set; }

    [Column("almacen_id")]
    public int AlmacenId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("nivel_minimo")]
    public int? NivelMinimo { get; set; }

    [Column("nivel_maximo")]
    public int? NivelMaximo { get; set; }

    [Column("nivel_reorden")]
    public int? NivelReorden { get; set; }

    [Column("cantidad_reorden")]
    public int? CantidadReorden { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("AlmacenId")]
    [InverseProperty("LgInventarios")]
    public virtual LgAlmacen Almacen { get; set; } = null!;

    [ForeignKey("ProductoId")]
    [InverseProperty("LgInventarios")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("ProductoVarianteId")]
    [InverseProperty("LgInventarios")]
    public virtual LgProductoVariante? ProductoVariante { get; set; }

    //lg_item_orden_compra
    [InverseProperty("Inventario")]
    public virtual ICollection<LgItemOrdenCompra> LgItemOrdenCompras { get; set; } = new HashSet<LgItemOrdenCompra>();



}

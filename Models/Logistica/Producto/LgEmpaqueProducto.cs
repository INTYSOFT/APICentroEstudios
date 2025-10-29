using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_empaque_producto")]
public partial class LgEmpaqueProducto
{
    [Key]
    [Column("empaque_producto_id")]
    public int EmpaqueProductoId { get; set; }

    [Column("producto_id")]
    public int? ProductoId { get; set; }

    [Column("empaque_id")]
    public int EmpaqueId { get; set; }

    [Column("parent_empaque_producto_id")]
    public int? ParentEmpaqueProductoId { get; set; }

    [Column("cantidad")]
    public short Cantidad { get; set; }

    [ForeignKey("EmpaqueId")]
    [InverseProperty("LgEmpaqueProductos")]
    public virtual LgEmpaque Empaque { get; set; } = null!;

    [InverseProperty("ParentEmpaqueProducto")]
    public virtual ICollection<LgEmpaqueProducto> InverseParentEmpaqueProducto { get; set; } = new List<LgEmpaqueProducto>();

    [ForeignKey("ParentEmpaqueProductoId")]
    [InverseProperty("InverseParentEmpaqueProducto")]
    public virtual LgEmpaqueProducto? ParentEmpaqueProducto { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("LgEmpaqueProductos")]
    public virtual LgProducto? Producto { get; set; }
}

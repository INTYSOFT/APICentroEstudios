using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[PrimaryKey("ProductoId", "AtributoProductoDetalleId", "Valor")]
[Table("lg_atributo_asignado_producto")]
public partial class LgAtributoAsignadoProducto
{
    [Key]
    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Key]
    [Column("atributo_producto_detalle_id")]
    public int AtributoProductoDetalleId { get; set; }

    [Key]
    [Column("valor")]
    [StringLength(128)]
    public string Valor { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("AtributoProductoDetalleId")]
    [InverseProperty("LgAtributoAsignadoProductos")]
    public virtual LgAtributoProductoDetalle AtributoProductoDetalle { get; set; } = null!;

    [ForeignKey("ProductoId")]
    [InverseProperty("LgAtributoAsignadoProductos")]
    public virtual LgProducto Producto { get; set; } = null!;
}

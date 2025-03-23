using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_detalle_guia_remision")]
public partial class LgDetalleGuiaRemision
{
    [Key]
    [Column("detalle_id")]
    public int DetalleId { get; set; }

    [Column("guia_id")]
    public int GuiaId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("variante_producto_id")]
    public int? VarianteProductoId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("peso")]
    [Precision(10, 2)]
    public decimal? Peso { get; set; }

    [Column("descripcion")]
    [StringLength(255)]
    public string Descripcion { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("GuiaId")]
    [InverseProperty("LgDetalleGuiaRemisions")]
    public virtual LgGuiaRemision Guia { get; set; } = null!;

    [ForeignKey("ProductoId")]
    [InverseProperty("LgDetalleGuiaRemisions")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("VarianteProductoId")]
    [InverseProperty("LgDetalleGuiaRemisions")]
    public virtual LgProductoVariante? VarianteProducto { get; set; }
}

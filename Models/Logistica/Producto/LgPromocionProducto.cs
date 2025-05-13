using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[PrimaryKey("ProductoId", "ProductoVarianteId", "PromocionId")]
[Table("lg_promocion_producto")]
public partial class LgPromocionProducto
{
    [Key]
    [Column("promocion_id")]
    public int PromocionId { get; set; }

    [Key]
    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("producto_variante_id")]
    public int? ProductoVarianteId  { get; set; }


    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("LgPromocionProductos")]
    public virtual LgProducto Producto { get; set; } = null!;

    [ForeignKey("PromocionId")]
    [InverseProperty("LgPromocionProductos")]
    public virtual LgPromocion Promocion { get; set; } = null!;

    //producto variante
    [ForeignKey("ProductoVarianteId")]
    [InverseProperty("LgPromocionProductos")]
    public virtual LgProductoVariante? ProductoVariante { get; set; }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_promocion")]
public partial class LgPromocion
{
    [Key]
    [Column("promocion_id")]
    public int PromocionId { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("porcentaje_descuento")]
    [Precision(5, 2)]
    public decimal PorcentajeDescuento { get; set; }

    [Column("fecha_inicio", TypeName = "timestamp without time zone")]
    public DateTime FechaInicio { get; set; }

    [Column("fecha_fin", TypeName = "timestamp without time zone")]
    public DateTime? FechaFin { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Promocion")]
    public virtual ICollection<LgPromocionProducto> LgPromocionProductos { get; set; } = new List<LgPromocionProducto>();
}

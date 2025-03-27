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

    [Column("producto_variante_id")]
    public int? ProductoVarianteId { get; set; }

    [Column("producto_presentacion_id")]
    public int? ProductoPresentacionId { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("LgDetalleGuiaRemisions")]
    public virtual LgProducto Producto { get; set; } = null!;

}

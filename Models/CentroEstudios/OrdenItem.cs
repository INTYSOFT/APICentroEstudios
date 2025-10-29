using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("orden_item", Schema = "academia")]
public partial class OrdenItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("orden_id")]
    public int OrdenId { get; set; }

    [Column("concepto_id")]
    public int ConceptoId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("precio_unit")]
    [Precision(12, 2)]
    public decimal PrecioUnit { get; set; }

    [Column("descuento")]
    [Precision(12, 2)]
    public decimal Descuento { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public int? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public int? UsuaraioActualizacionId { get; set; }

    [ForeignKey("ConceptoId")]
    [InverseProperty("OrdenItems")]
    public virtual Concepto Concepto { get; set; } = null!;

    [ForeignKey("OrdenId")]
    [InverseProperty("OrdenItems")]
    public virtual Orden Orden { get; set; } = null!;
}

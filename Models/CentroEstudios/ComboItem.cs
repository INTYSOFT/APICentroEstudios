using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("combo_item", Schema = "academia")]
[Index("ComboId", "ConceptoId", Name = "ux_combo_item_unique", IsUnique = true)]
public partial class ComboItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("combo_id")]
    public int ComboId { get; set; }

    [Column("concepto_id")]
    public int ConceptoId { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

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

    [ForeignKey("ComboId")]
    [InverseProperty("ComboItems")]
    public virtual Combo Combo { get; set; } = null!;

    [ForeignKey("ConceptoId")]
    [InverseProperty("ComboItems")]
    public virtual Concepto Concepto { get; set; } = null!;
}

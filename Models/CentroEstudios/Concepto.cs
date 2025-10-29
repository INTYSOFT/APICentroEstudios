using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("concepto", Schema = "academia")]
[Index("ConceptoTipoId", Name = "ix_concepto_tipo_id")]
public partial class Concepto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("precio")]
    [Precision(12, 2)]
    public decimal Precio { get; set; }

    [Column("impuesto")]
    [Precision(5, 2)]
    public decimal? Impuesto { get; set; }

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

    [Column("concepto_tipo_id")]
    public int? ConceptoTipoId { get; set; }

    //MatriculaItem
    [InverseProperty("Concepto")]
    public virtual ICollection<MatriculaItem> MatriculaItems { get; set; } = new List<MatriculaItem>();

    //Orden item
    [InverseProperty("Concepto")]
    public virtual ICollection<OrdenItem> OrdenItems { get; set; } = new List<OrdenItem>();
}

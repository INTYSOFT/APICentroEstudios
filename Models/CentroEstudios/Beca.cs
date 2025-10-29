using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("beca", Schema = "academia")]
[Index("Nombre", Name = "ux_beca_nombre", IsUnique = true)]
public partial class Beca
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("tipo")]
    public string Tipo { get; set; } = null!;

    [Column("valor")]
    [Precision(12, 2)]
    public decimal Valor { get; set; }

    [Column("tope")]
    [Precision(12, 2)]
    public decimal? Tope { get; set; }

    [Column("combinable")]
    public bool Combinable { get; set; }

    [Column("vigencia_inicio")]
    public DateOnly? VigenciaInicio { get; set; }

    [Column("vigencia_fin")]
    public DateOnly? VigenciaFin { get; set; }

    [Column("criterio_json", TypeName = "jsonb")]
    public string? CriterioJson { get; set; }

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

    [InverseProperty("Beca")]
    public virtual ICollection<BecaAlumno> BecaAlumnos { get; set; } = new List<BecaAlumno>();
}

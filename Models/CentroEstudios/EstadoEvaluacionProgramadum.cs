using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("estado_evaluacion_programada", Schema = "academia")]
[Index("Orden", Name = "ux_estado_ep_orden", IsUnique = true)]
public partial class EstadoEvaluacionProgramadum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("codigo")]
    public string Codigo { get; set; } = null!;

    [Column("orden")]
    public short Orden { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

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

    //EvaluacionProgramadas
    [InverseProperty("Estado")]
    public virtual ICollection<EvaluacionProgramadum>? EvaluacionProgramadas { get; set; } = new List<EvaluacionProgramadum>();
}
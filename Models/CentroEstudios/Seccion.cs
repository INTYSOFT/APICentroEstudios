using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("seccion", Schema = "academia")]
[Index("Nombre", Name = "ux_seccion_ciclo_nombre", IsUnique = true)]
public partial class Seccion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("capacidad")]
    public int Capacidad { get; set; }

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

    [InverseProperty("Seccion")]
    public virtual ICollection<Asistencium>? Asistencia { get; set; } = new List<Asistencium>();

    //EvaluacionProgramadaSeccions
    [InverseProperty("Seccion")]
    public virtual ICollection<EvaluacionProgramadaSeccion>? EvaluacionProgramadaSeccions { get; set; } = new List<EvaluacionProgramadaSeccion>();

    //EvaluacionDetalles
    [InverseProperty("Seccion")]
    public virtual ICollection<EvaluacionDetalle>? EvaluacionDetalles { get; set; } = new List<EvaluacionDetalle>();

    //evaliuaciones
    [InverseProperty("Seccion")]
    public virtual ICollection<Evaluacion>? Evaluacions { get; set; } = new List<Evaluacion>();

    //EvaluacionRespuesta
    [InverseProperty("Seccion")]
    public virtual ICollection<EvaluacionRespuestum>? EvaluacionRespuesta { get; set; } = new List<EvaluacionRespuestum>();



}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("ciclo", Schema = "academia")]
[Index("Nombre", IsUnique = true)]
public partial class Ciclo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }


    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    //fecha_apertura_inscripcion     date.
    [Column("fecha_apertura_inscripcion")]
    public DateOnly? FechaAperturaInscripcion { get; set; }


    //fecha_cierre_inscripcion date
    [Column("fecha_cierre_inscripcion")]
    public DateOnly? FechaCierreInscripcion { get; set; }

    [Column("fecha_fin")]
    public DateOnly FechaFin { get; set; }

    [Column("capacidad_total")]
    public int? CapacidadTotal { get; set; }

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

    //evaluacion
    [InverseProperty("Ciclo")]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();



}

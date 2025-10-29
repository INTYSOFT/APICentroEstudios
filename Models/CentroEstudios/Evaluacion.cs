using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion", Schema = "academia")]
[Index("EvaluacionProgramadaId", "AlumnoId", Name = "ux_eval_ep_alumno", IsUnique = true)]
public partial class Evaluacion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_programada_id")]
    public int EvaluacionProgramadaId { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    //sede_id
    [Column("sede_id")]
    public int? SedeId { get; set; }



    //ciclo_id
    [Column("ciclo_id")]
    public int? CicloId { get; set; }


    //seccion_id
    [Column("seccion_id")]
    public int? SeccionId { get; set; }



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



    [InverseProperty("Evaluacion")]
    public virtual EvaluacionNotum? EvaluacionNotum { get; set; }

    [ForeignKey("EvaluacionProgramadaId")]
    [InverseProperty("Evaluacions")]
    public virtual EvaluacionProgramadum? EvaluacionProgramada { get; set; } = null!;

    [InverseProperty("Evaluacion")]
    public virtual ICollection<EvaluacionRespuestum>? EvaluacionRespuesta { get; set; } = new List<EvaluacionRespuestum>();

    //alumno
    [ForeignKey("AlumnoId")]
    [InverseProperty("Evaluacions")]
    public virtual Alumno? Alumno { get; set; } = null!;

    //sedeId.
    [ForeignKey("SedeId")]
    [InverseProperty("Evaluacions")]
    public virtual Sede? Sede { get; set; }

    //cicloId
    [ForeignKey("CicloId")]
    [InverseProperty("Evaluacions")]
    public virtual Ciclo? Ciclo { get; set; }

    //seccionId
    [ForeignKey("SeccionId")]
    [InverseProperty("Evaluacions")]
    public virtual Seccion? Seccion { get; set; }









}

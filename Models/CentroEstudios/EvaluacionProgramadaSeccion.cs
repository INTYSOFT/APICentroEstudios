using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_programada_seccion", Schema = "academia")]
[Index("EvaluacionProgramadaId", Name = "ix_eps_ep")]
[Index("SeccionCicloId", Name = "ix_eps_sc")]
[Index("SeccionId", Name = "ix_eps_seccion")]
[Index("EvaluacionProgramadaId", "SeccionCicloId", Name = "ux_eps_ep_sc", IsUnique = true)]
public partial class EvaluacionProgramadaSeccion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_programada_id")]
    public int EvaluacionProgramadaId { get; set; }

    [Column("seccion_ciclo_id")]
    public int SeccionCicloId { get; set; }

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

    // Navigation properties
    [ForeignKey("EvaluacionProgramadaId")]
    [InverseProperty("EvaluacionProgramadaSeccions")]
    public virtual EvaluacionProgramadum? EvaluacionProgramada { get; set; } = null!;

    [ForeignKey("SeccionCicloId")]
    [InverseProperty("EvaluacionProgramadaSeccions")]
    public virtual SeccionCiclo? SeccionCiclo { get; set; } = null!;

    //seccion
    [ForeignKey("SeccionId")]
    [InverseProperty("EvaluacionProgramadaSeccions")]
    public virtual Seccion? Seccion { get; set; } = null!;

}




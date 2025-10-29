using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("apertura_seccion", Schema = "academia")]
[Index("Activo", Name = "ix_aps_activo")]
[Index("AperturaCicloId", Name = "ix_aps_apertura_ciclo")]
[Index("NivelId", Name = "ix_aps_nivel")]
[Index("SeccionId", Name = "ix_aps_seccion")]
[Index("AperturaCicloId", "SeccionId", Name = "ux_apertura_seccion_unica", IsUnique = true)]
public partial class AperturaSeccion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("apertura_ciclo_id")]
    public int AperturaCicloId { get; set; }

    [Column("seccion_id")]
    public int SeccionId { get; set; }

    [Column("nivel_id")]
    public int? NivelId { get; set; }

    [Column("capacidad")]
    public int? Capacidad { get; set; }

    [Column("aforo_maximo")]
    public int? AforoMaximo { get; set; }

    [Column("aula")]
    public string? Aula { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }

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
}

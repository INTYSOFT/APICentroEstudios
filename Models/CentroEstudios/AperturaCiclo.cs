using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("apertura_ciclo", Schema = "academia")]
[Index("CicloId", Name = "ix_apc_ciclo")]
[Index("SedeId", Name = "ix_apc_sede")]
[Index("SedeId", "CicloId", Name = "ux_apertura_ciclo_unica", IsUnique = true)]
public partial class AperturaCiclo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sede_id")]
    public int SedeId { get; set; }

    [Column("ciclo_id")]
    public int CicloId { get; set; }

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

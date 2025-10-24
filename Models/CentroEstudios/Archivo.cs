using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("archivo", Schema = "academia")]
[Index("OwnerTable", "OwnerId", Name = "ix_archivo_owner")]
public partial class Archivo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("owner_table")]
    public string? OwnerTable { get; set; }

    [Column("owner_id")]
    public int? OwnerId { get; set; }

    [Column("tipo")]
    public string? Tipo { get; set; }

    [Column("url")]
    public string Url { get; set; } = null!;

    [Column("sha256")]
    public string? Sha256 { get; set; }

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

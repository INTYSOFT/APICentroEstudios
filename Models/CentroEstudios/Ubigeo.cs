using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("ubigeo", Schema = "academia")]
[Index("Code", Name = "ux_ubigeo_code", IsUnique = true)]
public partial class Ubigeo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(6)]
    public string Code { get; set; } = null!;

    [Column("departamento")]
    public string Departamento { get; set; } = null!;

    [Column("provincia")]
    public string Provincia { get; set; } = null!;

    [Column("distrito")]
    public string Distrito { get; set; } = null!;

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

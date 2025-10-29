using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("apoderado", Schema = "academia")]
public partial class Apoderado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("apellidos")]
    public string? Apellidos { get; set; }

    [Column("nombres")]
    public string? Nombres { get; set; }

    [Column("celular")]
    public string? Celular { get; set; }

    [Column("correo")]
    public string? Correo { get; set; }

    [Column("documento")]
    [StringLength(15)]
    public string? Documento { get; set; }

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

    [InverseProperty("Apoderado")]
    public virtual ICollection<AlumnoApoderado> AlumnoApoderados { get; set; } = new List<AlumnoApoderado>();
}

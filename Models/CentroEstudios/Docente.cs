using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("docente", Schema = "academia")]
[Index("Dni", Name = "ux_docente_dni", IsUnique = true)]
public partial class Docente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dni")]
    [StringLength(12)]
    public string Dni { get; set; } = null!;

    [Column("apellidos")]
    public string? Apellidos { get; set; }

    [Column("nombres")]
    public string? Nombres { get; set; }

    [Column("celular")]
    public string? Celular { get; set; }

    [Column("correo")]
    public string? Correo { get; set; }

    //especialidad_id
    [Column("especialidad_id")]
    public int? EspecialidadId { get; set; }


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


    [InverseProperty("Docente")]
    public virtual DocenteUsuario? DocenteUsuario { get; set; }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("docente_usuario", Schema = "academia")]
[Index("DocenteId", Name = "ux_du_docente", IsUnique = true)]
[Index("UsuarioId", Name = "ux_du_usuario", IsUnique = true)]
public partial class DocenteUsuario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("docente_id")]
    public int DocenteId { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

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

    [ForeignKey("DocenteId")]
    [InverseProperty("DocenteUsuario")]
    public virtual Docente Docente { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("DocenteUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;
}

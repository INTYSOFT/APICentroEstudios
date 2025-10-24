using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace api_intiSoft.Models.CentroEstudios;

[Table("audit_log", Schema = "academia")]
[Index("FechaRegistro", Name = "ix_audit_creado", AllDescending = true)]
[Index("Entidad", "EntidadId", Name = "ix_audit_entidad")]
public partial class AuditLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("entidad")]
    public string Entidad { get; set; } = null!;

    [Column("entidad_id")]
    public int EntidadId { get; set; }

    [Column("accion")]
    public string Accion { get; set; } = null!;

    [Column("usuario_id")]
    public int? UsuarioId { get; set; }

    [Column("ip")]
    public IPAddress? Ip { get; set; }

    [Column("diff_json", TypeName = "jsonb")]
    public string? DiffJson { get; set; }

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

    [ForeignKey("UsuarioId")]
    [InverseProperty("AuditLogs")]
    public virtual Usuario? Usuario { get; set; }
}

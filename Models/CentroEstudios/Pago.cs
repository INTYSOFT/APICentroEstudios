using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("pago", Schema = "academia")]
public partial class Pago
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("orden_id")]
    public int OrdenId { get; set; }

    [Column("monto")]
    [Precision(12, 2)]
    public decimal Monto { get; set; }

    [Column("transaccion_ref")]
    public string? TransaccionRef { get; set; }

    [Column("emitido_a_sunat")]
    public bool EmitidoASunat { get; set; }

    [Column("sunat_payload", TypeName = "jsonb")]
    public string? SunatPayload { get; set; }

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

    [InverseProperty("Pago")]
    public virtual ICollection<Devolucion> Devolucions { get; set; } = new List<Devolucion>();

    [ForeignKey("OrdenId")]
    [InverseProperty("Pagos")]
    public virtual Orden Orden { get; set; } = null!;
}

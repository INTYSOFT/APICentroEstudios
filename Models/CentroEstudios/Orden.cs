using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("orden", Schema = "academia")]
public partial class Orden
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("matricula_id")]
    public int MatriculaId { get; set; }

    [Column("sede_id")]
    public int SedeId { get; set; }

    [Column("monto_total")]
    [Precision(12, 2)]
    public decimal MontoTotal { get; set; }

    [Column("moneda")]
    [StringLength(3)]
    public string Moneda { get; set; } = null!;

    [Column("referencia_pago")]
    public string? ReferenciaPago { get; set; }

    [Column("comprobante_json", TypeName = "jsonb")]
    public string? ComprobanteJson { get; set; }

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

    [ForeignKey("MatriculaId")]
    [InverseProperty("Ordens")]
    public virtual Matricula Matricula { get; set; } = null!;

    [InverseProperty("Orden")]
    public virtual ICollection<OrdenItem> OrdenItems { get; set; } = new List<OrdenItem>();

    [InverseProperty("Orden")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [ForeignKey("SedeId")]
    [InverseProperty("Ordens")]
    public virtual Sede Sede { get; set; } = null!;
}

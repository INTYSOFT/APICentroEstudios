using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Contabilidad;

[Table("ct_documentos_tributarios")]
[Index("NumeroDocumento", Name = "ct_documentos_tributarios_numero_documento_key", IsUnique = true)]
public partial class CtDocumentosTributario
{
    [Key]
    [Column("documento_id")]
    public int DocumentoId { get; set; }

    [Column("comprobante_id")]
    [StringLength(2)]
    public string ComprobanteId { get; set; } = null!;

    [Column("estado_documento_id")]
    public int EstadoDocumentoId { get; set; }

    [Column("serie")]
    [StringLength(4)]
    public string Serie { get; set; } = null!;

    [Column("numero_documento")]
    [StringLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("fecha_emision", TypeName = "timestamp without time zone")]
    public DateTime FechaEmision { get; set; }

    [Column("cliente_id")]
    public int? ClienteId { get; set; }

    [Column("proveedor_id")]
    public int? ProveedorId { get; set; }

    [Column("monto_total")]
    [Precision(15, 2)]
    public decimal MontoTotal { get; set; }

    [Column("igv")]
    [Precision(15, 2)]
    public decimal Igv { get; set; }

    [Column("total")]
    [Precision(15, 2)]
    public decimal Total { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [ForeignKey("ComprobanteId")]
    [InverseProperty("CtDocumentosTributarios")]
    public virtual CtComprobante Comprobante { get; set; } = null!;

    [ForeignKey("EstadoDocumentoId")]
    [InverseProperty("CtDocumentosTributarios")]
    public virtual CtEstadoDocumento EstadoDocumento { get; set; } = null!;
}

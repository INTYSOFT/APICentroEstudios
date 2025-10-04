using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Contratos con proveedores (marco/servicio) con montos, vigencias y enlaces al documento.
/// </summary>
[Table("contrato", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_contrato_prov")]
[Index("TenantId", "Codigo", Name = "uq_contrato_codigo", IsUnique = true)]
public partial class Contrato
{
    [Key]
    [Column("contrato_id")]
    public Guid ContratoId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("codigo")]
    public string Codigo { get; set; } = null!;

    [Column("titulo")]
    public string Titulo { get; set; } = null!;

    [Column("monto_maximo")]
    [Precision(18, 2)]
    public decimal? MontoMaximo { get; set; }

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly? FechaFin { get; set; }

    [Column("url_contrato")]
    public string? UrlContrato { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public Guid? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public Guid? UsuaraioActualizacionId { get; set; }

    [InverseProperty("Contrato")]
    public virtual ICollection<ContratoClausula> ContratoClausulas { get; set; } = new List<ContratoClausula>();

    [ForeignKey("ProveedorId")]
    [InverseProperty("Contratos")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}

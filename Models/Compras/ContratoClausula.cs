using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Cláusulas por contrato con versión, obligatoriedad y texto completo.
/// </summary>
[Table("contrato_clausula", Schema = "compras")]
[Index("TenantId", "ContratoId", Name = "ix_contrato_clausula_contrato")]
public partial class ContratoClausula
{
    [Key]
    [Column("contrato_clausula_id")]
    public Guid ContratoClausulaId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("contrato_id")]
    public Guid ContratoId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("version")]
    public string Version { get; set; } = null!;

    [Column("es_obligatoria")]
    public bool EsObligatoria { get; set; }

    [Column("texto")]
    public string Texto { get; set; } = null!;

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

    [ForeignKey("ContratoId")]
    [InverseProperty("ContratoClausulas")]
    public virtual Contrato Contrato { get; set; } = null!;
}

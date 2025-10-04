using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Registro de homologación por proveedor (estado, puntaje, vigencias) por categoría/criticidad.
/// </summary>
[Table("proveedor_homologacion", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_homologacion_prov")]
public partial class ProveedorHomologacion
{
    [Key]
    [Column("proveedor_homologacion_id")]
    public Guid ProveedorHomologacionId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [Column("puntaje_total")]
    [Precision(6, 2)]
    public decimal? PuntajeTotal { get; set; }

    [Column("vigente_desde")]
    public DateOnly? VigenteDesde { get; set; }

    [Column("vigente_hasta")]
    public DateOnly? VigenteHasta { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }

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

    [ForeignKey("ProveedorId")]
    [InverseProperty("ProveedorHomologacions")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}

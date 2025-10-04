using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Vinculación del proveedor a categorías del catálogo corporativo (LG_CATEGORIA). Permite múltiples categorías por proveedor.
/// </summary>
[Table("proveedor_categoria", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_categoria_prov")]
[Index("TenantId", "ProveedorId", "CategoriaId", Name = "uq_prov_cat", IsUnique = true)]
public partial class ProveedorCategorium
{
    [Key]
    [Column("proveedor_categoria_id")]
    public Guid ProveedorCategoriaId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    [Column("prioridad")]
    public short Prioridad { get; set; }

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
    [InverseProperty("ProveedorCategoria")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}

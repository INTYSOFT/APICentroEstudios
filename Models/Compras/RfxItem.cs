using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Ítems del RFx; opcionalmente referenciados a productos existentes y categorías del catálogo.
/// </summary>
[Table("rfx_item", Schema = "compras")]
[Index("TenantId", "RfxId", Name = "ix_rfx_item_rfx")]
public partial class RfxItem
{
    [Key]
    [Column("rfx_item_id")]
    public Guid RfxItemId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("rfx_id")]
    public Guid RfxId { get; set; }

    [Column("descripcion")]
    public string Descripcion { get; set; } = null!;

    [Column("cantidad")]
    [Precision(18, 6)]
    public decimal Cantidad { get; set; }

    [Column("unidad")]
    public string Unidad { get; set; } = null!;

    [Column("producto_variante_presentacion_id")]
    public int? ProductoVariantePresentacionId { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

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

    [ForeignKey("RfxId")]
    [InverseProperty("RfxItems")]
    public virtual Rfx Rfx { get; set; } = null!;
}

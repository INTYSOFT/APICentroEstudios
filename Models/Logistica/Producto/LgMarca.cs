using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_marca")]
[Index("CodigoMarca", Name = "idx_lg_marca_codigo")]
[Index("Nombre", Name = "idx_lg_marca_nombre")]
[Index("CodigoMarca", Name = "lg_marca_codigo_marca_key", IsUnique = true)]
public partial class LgMarca
{
    [Key]
    [Column("marca_id")]
    public int MarcaId { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("codigo_marca")]
    [StringLength(50)]
    public string? CodigoMarca { get; set; }

    [Column("logo_url")]
    [StringLength(255)]
    public string? LogoUrl { get; set; }

    [Column("sitio_web")]
    [StringLength(255)]
    public string? SitioWeb { get; set; }

    [Column("numero_registro")]
    [StringLength(50)]
    public string? NumeroRegistro { get; set; }

    [Column("tipo_marca_id")]
    public int? TipoMarcaId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("fecha_actualizacion", TypeName = "timestamp without time zone")]
    public DateTime? FechaActualizacion { get; set; } = DateTime.UtcNow;

    [Column("activo")]
    public bool? Activo { get; set; }
}

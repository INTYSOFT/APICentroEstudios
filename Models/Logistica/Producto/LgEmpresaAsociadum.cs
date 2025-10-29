using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_empresa_asociada")]
[Index("Ruc", Name = "lg_empresa_asociada_ruc_key", IsUnique = true)]
public partial class LgEmpresaAsociadum
{
    [Key]
    [Column("empresa_asociada_id")]
    public int EmpresaAsociadaId { get; set; }

    [Column("ruc")]
    [StringLength(11)]
    public string Ruc { get; set; } = null!;

    [Column("razon_social")]
    [StringLength(255)]
    public string RazonSocial { get; set; } = null!;

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("page_web")]
    [StringLength(100)]
    public string? PageWeb { get; set; }

    [Column("representante_legal")]
    [StringLength(255)]
    public string? RepresentanteLegal { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }
}

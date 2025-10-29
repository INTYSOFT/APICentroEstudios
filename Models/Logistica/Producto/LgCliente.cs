using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_cliente")]
public partial class LgCliente
{
    [Key]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("documento_identidad_id")]
    [StringLength(3)]
    public string DocumentoIdentidadId { get; set; } = null!;

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("geo_latitud_llegada")]
    [Precision(10, 6)]
    public decimal GeoLatitudLlegada { get; set; }

    [Column("geo_longitud_llegada")]
    [Precision(10, 6)]
    public decimal GeoLongitudLlegada { get; set; }

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("page_web")]
    [StringLength(100)]
    public string? PageWeb { get; set; }

    [Column("saldo_cuenta")]
    [Precision(10, 2)]
    public decimal? SaldoCuenta { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("DocumentoIdentidadId")]
    [InverseProperty("LgClientes")]
    public virtual LgDocumentoIdentidad DocumentoIdentidad { get; set; } = null!;

    [InverseProperty("Cliente")]
    public virtual ICollection<LgContactoCliente> LgContactoClientes { get; set; } = new List<LgContactoCliente>();

    [InverseProperty("Cliente")]
    public virtual ICollection<LgVentum> LgVenta { get; set; } = new List<LgVentum>();
}

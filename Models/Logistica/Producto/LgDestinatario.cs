using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_destinatario")]
public partial class LgDestinatario
{
    [Key]
    [Column("destinatario_id")]
    public int DestinatarioId { get; set; }

    [Column("nombre_razon_social")]
    [StringLength(255)]
    public string NombreRazonSocial { get; set; } = null!;

    [Column("tipo_documento")]
    [StringLength(3)]
    public string TipoDocumento { get; set; } = null!;

    [Column("geo_latitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLatitudOrigen { get; set; }

    [Column("geo_longitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLongitudOrigen { get; set; }

    [Column("numero_documento")]
    [StringLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("direccion")]
    [StringLength(255)]
    public string Direccion { get; set; } = null!;

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [InverseProperty("Destinatario")]
    public virtual ICollection<LgGuiaRemision> LgGuiaRemisions { get; set; } = new List<LgGuiaRemision>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_transportista")]
[Index("NumeroDocumento", Name = "lg_transportista_numero_documento_key", IsUnique = true)]
public partial class LgTransportistum
{
    [Key]
    [Column("transportista_id")]
    public int TransportistaId { get; set; }

    [Column("tipo_documento")]
    [StringLength(3)]
    public string TipoDocumento { get; set; } = null!;

    [Column("numero_documento")]
    [StringLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("nombre_razon_social")]
    [StringLength(255)]
    public string NombreRazonSocial { get; set; } = null!;

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Transportista")]
    public virtual ICollection<LgGuiaRemision> LgGuiaRemisions { get; set; } = new List<LgGuiaRemision>();
}

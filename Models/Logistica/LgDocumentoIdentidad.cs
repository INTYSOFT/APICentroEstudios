using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_intiSoft.Models.Logistica.Producto;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica;

[Table("lg_documento_identidad")]
public partial class LgDocumentoIdentidad
{
    [Key]
    [Column("documento_identidad_id")]
    [StringLength(3)]
    public string DocumentoIdentidadId { get; set; } = null!;

    [Column("documento_identidad")]
    [StringLength(50)]
    public string DocumentoIdentidad { get; set; } = null!;

    [Column("aliass")]
    [StringLength(8)]
    public string? Aliass { get; set; }

    [Column("descripcion")]
    [StringLength(100)]
    public string? Descripcion { get; set; }

    [Column("longuitud")]
    public short Longuitud { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("DocumentoIdentidad")]
    public virtual ICollection<LgProveedor> Proveedores { get; set; } = new List<LgProveedor>();

    [InverseProperty("DocumentoIdentidad")]
    public virtual ICollection<LgCliente> Clientes { get; set; } = new List<LgCliente>();

}

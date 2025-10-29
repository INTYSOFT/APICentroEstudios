using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_contacto")]
public partial class LgContacto
{
    [Key]
    [Column("contacto_id")]
    public int ContactoId { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

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

    [InverseProperty("Contacto")]
    public virtual ICollection<LgContactoCliente> LgContactoClientes { get; set; } = new List<LgContactoCliente>();
}

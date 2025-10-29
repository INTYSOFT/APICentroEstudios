using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_contacto_cliente")]
public partial class LgContactoCliente
{
    [Key]
    [Column("contacto_cliente_id")]
    public int ContactoClienteId { get; set; }

    [Column("contacto_id")]
    public int ContactoId { get; set; }

    [Column("tipo_contacto_id")]
    public int TipoContactoId { get; set; }

    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("LgContactoClientes")]
    public virtual LgCliente Cliente { get; set; } = null!;

    [ForeignKey("ContactoId")]
    [InverseProperty("LgContactoClientes")]
    public virtual LgContacto Contacto { get; set; } = null!;

    [ForeignKey("TipoContactoId")]
    [InverseProperty("LgContactoClientes")]
    public virtual LgTipoContacto TipoContacto { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Configuracion;

[Table("cf_producto")]
public partial class CfProducto
{
    [Column("with_presentacion_sku")]
    public bool? WithPresentacionSku { get; set; }

    [Column("with_codigo_barras")]
    public bool? WithCodigoBarras { get; set; }

    [Column("with_codigo_qr")]
    public bool? WithCodigoQr { get; set; }

    [Column("with_codigo_interno")]
    public bool? WithCodigoInterno { get; set; }

    [Column("with_variantes")]
    public bool? WithVariantes { get; set; }

    [Column("with_dimensiones_peso")]
    public bool? WithDimensionesPeso { get; set; }

    [Column("with_gestion_incluye_presentacion")]
    public bool? WithGestionIncluyePresentacion { get; set; }

    [Key]
    [Column("producto_config_id")]
    public int ProductoConfigId { get; set; }

    //with_empaque
    [Column("with_empaque")]
    public bool? WithEmpaque { get; set; }

    //with_marca
    [Column("with_marca")]
    public bool? WithMarca { get; set; }

    //with_modelo
    [Column("with_modelo")]
    public bool? WithModelo { get; set; }

    //with_tiempo_servicio
    [Column("with_tiempo_servicio")]
    public bool? WithTiempoServicio { get; set; }
}

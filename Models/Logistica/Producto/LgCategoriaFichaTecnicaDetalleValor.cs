using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_categoria_ficha_tecnica_detalle_valor")]
public partial class LgCategoriaFichaTecnicaDetalleValor
{
        [Key]
        [Column("categoria_ficha_tecnica_detalle_valor_id")]
        public int CategoriaFichaTecnicaDetalleValorId { get; set; }

        [ForeignKey("CategoriaFichaTecnicaDetalle")]
        [Column("categoria_ficha_tecnica_detalle_id")]
        public int CategoriaFichaTecnicaDetalleId { get; set; }
        public virtual LgCategoriaFichaTecnicaDetalle? CategoriaFichaTecnicaDetalle { get; set; }

        [Required]
        [Column("valor")]
        [StringLength(512)]
        public string Valor { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Required]
        [Column("version")]
        public int? Version { get; set; } = 1;

        [Required]
        [Column("activo")]
        public bool Activo { get; set; } = true;
    }
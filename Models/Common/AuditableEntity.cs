using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.Common
{
    public class AuditableEntity : IAuditableEntity
    {
        [Column("fecha_registro")]
        public DateTime? FechaRegistro { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime? FechaActualizacion { get; set; }


        [Column("usuaraio_registro_id")]
        public int? UsuaraioRegistroId { get; set; }

        [Column("usuaraio_actualizacion_id")]
        public int? UsuaraioActualizacionId { get; set; }
    }
}

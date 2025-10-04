namespace api_intiSoft.Models.Common
{
    public interface IAuditableEntity
    {
        DateTime? FechaRegistro { get; set; }
        DateTime? FechaActualizacion { get; set; }
        int? UsuaraioRegistroId { get; set; }
        int? UsuaraioActualizacionId { get; set; }
    }
}

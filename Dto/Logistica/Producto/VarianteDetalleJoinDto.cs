namespace api_intiSoft.Dto.Logistica.Producto
{
    public record VarianteDetalleJoinDto(
        // Campos de V (cabecera)
        int? ListaFichaTecnicaId,
        string? NombreVariante,
        // Campos de D (detalle)
        int ProductoVarianteDetalleId,
        int ProductoVarianteId,
        int? DetalleListaFichaTecnicaId,
        string? Dato,
        string? Descripcion,
        bool? Activo
    );
}

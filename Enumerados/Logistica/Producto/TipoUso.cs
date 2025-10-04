using NpgsqlTypes;

namespace api_intiSoft.Enumerados.Logistica.Producto
{
    public enum TipoUso
    {
        [PgName("Solo Ventas")]
        SoloVentas,

        [PgName("Solo Compras")]
        SoloCompras,

        [PgName("Compras y Ventas")]
        ComprasYVentas
    }
}

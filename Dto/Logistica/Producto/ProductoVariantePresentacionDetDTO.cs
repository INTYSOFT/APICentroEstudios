using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Dto.Logistica.Producto
{
    public class ProductoVariantePresentacionDetDTO
    {
        public int? ProductoPresentacionId { get; set; }
        public ProductoVariantePresentacionDet Detalle { get; set; } = null!;
    }
}

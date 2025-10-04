using api_intiSoft.Dto.Logistica.Producto;
using api_intiSoft.Models.Logistica.Producto;
using AutoMapper;

namespace api_intiSoft.Service.Logistica.Producto
{
    public class LgProductoVarianteProfile : Profile
    {
        public LgProductoVarianteProfile()
        {
            CreateMap<LgProductoVarianteDto, ProductoVariante>();
            CreateMap<ProductoVariante, LgProductoVarianteDto>();
        }
    }
}

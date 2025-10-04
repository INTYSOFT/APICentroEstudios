using api_intiSoft.Dto.Logistica.Producto;
using api_intiSoft.Models.Logistica.Producto;
using AutoMapper;

namespace api_intiSoft.Service.Logistica.Producto 
{
    public class LgProductoFichaTecnicaProfile : Profile
    {
        public LgProductoFichaTecnicaProfile()
        {
            CreateMap<LgProductoFichaTecnicaDto, ProductoFichaTecnica>();
            CreateMap<ProductoFichaTecnica, LgProductoFichaTecnicaDto>();
        }
    }
}

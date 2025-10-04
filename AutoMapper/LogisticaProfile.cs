using AutoMapper;
using api_intiSoft.Models.Logistica.Producto;
using api_intiSoft.Dto.Logistica.Producto;

namespace api_intiSoft.AutoMapper
{
    public class LogisticaProfile : Profile
    {
        public LogisticaProfile()
        {
            // Modelo → DTO
            CreateMap<ProductoVariante, LgProductoVarianteDto>();
            CreateMap<ProductoVarianteDetalle, LgProductoVarianteDetalleDto>();

            //ProductoPresentacion
            CreateMap<ProductoPresentacion, LgProductoPresentacionDto>();


            //CreateMap<ProductoFichaTecnica, LgProductoFichaTecnicaDto>();
            CreateMap<ProductoFichaTecnica, LgProductoFichaTecnicaDto>()
                .ForMember(d => d.LgProductoFichaTecnicaDetalles,
                           o => o.MapFrom(s => s.ProductoFichaTecnicaDetalles));

            CreateMap<ProductoFichaTecnicaDetalle, LgProductoFichaTecnicaDetalleDto>();

            //LgProductoVariantePresentacion
            //CreateMap<LgProductoVariantePresentacion, LgProductoVariantePresentacionDto>();
            CreateMap<ProductoVariantePresentacion, LgProductoVariantePresentacionDto>()
               // Evitar intentar convertir NpgsqlTsVector → string/object
               .ForMember(d => d.NombreBusqueda, opt => opt.Ignore());

            // DTO → Modelo
            CreateMap<LgProductoVarianteDto, ProductoVariante>();
            CreateMap<LgProductoVarianteDetalleDto, ProductoVarianteDetalle>();
            

            //ProductoPresentacion
            CreateMap<LgProductoPresentacionDto, ProductoPresentacion>();

            //CreateMap<LgProductoFichaTecnicaDto, ProductoFichaTecnica>();
            //CreateMap<LgProductoFichaTecnicaDetalleDto, ProductoFichaTecnicaDetalle>();


            // Cabecera DTO -> Entidad (mapea colección al nombre real + ignora navegaciones)
            CreateMap<LgProductoFichaTecnicaDto, ProductoFichaTecnica>()
                .ForMember(d => d.ProductoFichaTecnicaId, o => o.Ignore())
                .ForMember(d => d.Producto, o => o.Ignore())
                .ForMember(d => d.FichaTecnica, o => o.Ignore())
                .ForMember(d => d.CategoriaFichaTecnica, o => o.Ignore())
                .ForMember(d => d.CategoriaFichaTecnicaDetalle, o => o.Ignore())
                .ForMember(d => d.ListaFichaTecnica, o => o.Ignore())
                .ForMember(d => d.ProductoFichaTecnicaDetalles,
                           o => o.MapFrom(s => s.LgProductoFichaTecnicaDetalles));            

            //producto ficha tecnica 
            //Detalle DTO -> Entidad (ignora FK a cabecera y navegaciones)
            CreateMap<LgProductoFichaTecnicaDetalleDto, ProductoFichaTecnicaDetalle>()
                .ForMember(d => d.ProductoFichaTecnicaId, o => o.Ignore())   // la pone EF por la relación
                .ForMember(d => d.ProductoFichaTecnica, o => o.Ignore())
                .ForMember(d => d.DetalleListaFichaTecnica, o => o.Ignore());

            //CreateMap<LgProductoVariantePresentacionDto, LgProductoVariantePresentacion>();
            CreateMap<LgProductoVariantePresentacionDto, ProductoVariantePresentacion>()
               // Si usas la tabla puente para múltiples detalles, el singular va NULL
               .ForMember(d => d.ProductoVarianteDetalleId, opt => opt.MapFrom(_ => (int?)null))
               // NUNCA intentes mapear el tsvector desde el DTO
               .ForMember(d => d.NombreBusqueda, opt => opt.Ignore())
               // Campos de auditoría los gestiona el contexto, no el DTO
               .ForMember(d => d.FechaRegistro, opt => opt.Ignore())
               .ForMember(d => d.FechaActualizacion, opt => opt.Ignore())
               .ForMember(d => d.UsuaraioRegistroId, opt => opt.Ignore())
               .ForMember(d => d.UsuaraioActualizacionId, opt => opt.Ignore());
        }
    }
}

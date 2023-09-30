using Api.Dtos;
using AutoMapper;
using Core.Entities;

namespace Api.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Producto, DtoProducto>()
                .ReverseMap();

            CreateMap<Categoria, DtoCategoria>()
                .ReverseMap();

            CreateMap<Marca, DtoMarca>()
                .ReverseMap();

            CreateMap<Producto, DtoProductoList>()
                .ForMember(des => des.Marca, origen => origen.MapFrom(origen => origen.Marca.Nombre))
                .ForMember(des => des.Categoria, origen => origen.MapFrom(origen => origen.Categoria.Nombre))
                .ReverseMap()
                .ForMember(origen => origen.Categoria, des => des.Ignore())
                .ForMember(origen => origen.Marca, des => des.Ignore());

        }

    }
}

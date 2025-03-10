using AutoMapper;
using webapi.Domain.Entities;
using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;

namespace webapi.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
     {
        CreateMap<Product, ProductListResponse>();
        CreateMap<Product, ProductDetailsResponse>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
            .ForMember(dest => dest.Colours, opt => opt.MapFrom(src => src.ProductColours.Select(pc => pc.Colour.ProductColours)));
        CreateMap<ProductRequest, Product>()
            .ForMember(dest => dest.ProductColours, opt => opt.MapFrom(src => src.ColourIds.Select(colourId => new ProductColour { ColourId = colourId })));
    }
}

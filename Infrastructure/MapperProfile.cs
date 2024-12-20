using AutoMapper;
using HtmlTemplate_MVC.DMO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Description, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<ProductDTO, ProductViewModel>().ReverseMap();
    }
}
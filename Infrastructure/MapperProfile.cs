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

        // Category siniflari icin mapleme
        CreateMap<ProductCategory, CategoryDTO>().ReverseMap();
        CreateMap<CategoryDTO, CategoryVM>().ReverseMap();

        // SubCategory siniflari icin mapleme
        CreateMap<ProductSubcategory, SubCategoryDTO>().ReverseMap();
        CreateMap<SubCategoryDTO, SubCategoryVM>().ReverseMap();


        CreateMap<ShopIndexVM, ShopIndexDTO>().ReverseMap();
        
         // Filter maplendi
        CreateMap<FilterDTO, ShopIndexVM>().ReverseMap();

    }
}
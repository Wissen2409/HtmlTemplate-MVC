using AutoMapper;
using HtmlTemplate_MVC.DataAccessLayer;
using HtmlTemplate_MVC.DMO;
using Microsoft.AspNetCore.Authorization.Infrastructure;

public class ProductCategoryRepository : IProductCategoryRepository
{

    private AdventureWorksContext _context;
    private IMapper _mapper;

    public ProductCategoryRepository(AdventureWorksContext context , IMapper mapper)
    {      
        _context = context;
        _mapper = mapper;

    }


    public List<CategoryDTO> GetAll()

    {        
        var categories = _context.ProductCategories.ToList();
        return _mapper.Map<List<CategoryDTO>>(categories);
    }

}


public interface IProductCategoryRepository
{
    public List<CategoryDTO> GetAll();

}
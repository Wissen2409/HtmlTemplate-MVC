public class ProductCategoryService : IProductCategoryService
{
    private IProductCategoryRepository _productCategoryRepository;  
    public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
        
    }
    public List<CategoryDTO> GetAll()
    {
       return _productCategoryRepository.GetAll();
    }
}

public interface IProductCategoryService
{
    List<CategoryDTO> GetAll();
}
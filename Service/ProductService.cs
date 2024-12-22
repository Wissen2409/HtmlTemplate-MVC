
public interface IProductService
{
    public List<ProductDTO> GetProducts(int productCount, int categoryId = 0, int subCategoryId = 0);
    public ProductDTO ProductDetail(int productid);
    public List<CategoryDTO> GetCategories();
    public ShopIndexDTO FilterCategoriesAndSubCategories(ShopIndexDTO model);
}
public class ProductService : IProductService
{
    // servis katmaninda sadece is plani uygulanacagi icin mapleme islemini burda yapmasak daha iyi olur?
    private IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<ProductDTO> GetProducts(int productCount, int categoryId = 0, int subCategoryId = 0)
    {
        return _productRepository.GetProductsByCategoryandSubCategory(productCount, categoryId, subCategoryId);
    }

    public ProductDTO ProductDetail(int productid)
    {
        return _productRepository.ProductDetail(productid);
    }

    public List<CategoryDTO> GetCategories()
    {
        return _productRepository.GetAllCategories();
    }

    public ShopIndexDTO FilterCategoriesAndSubCategories(ShopIndexDTO model)
    {
        model.Categories = _productRepository.GetAllCategories();
        model.SubCategories = _productRepository.GetSubCategoriesByCategoryId(model.SelCategoryId);
        model.Products = _productRepository.GetProductsByCategoryandSubCategory(12, model.SelCategoryId, model.SelSubCategoryId);
        return model;
    }
}
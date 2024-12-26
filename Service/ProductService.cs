
public interface IProductService
{
    public List<ProductDTO> GetProducts(int productCount, int categoryId = 0, int subCategoryId = 0);
    public ProductDTO ProductDetail(int productid);
    public List<CategoryDTO> GetCategories();
    public List<SubCategoryDTO> GetSubCategoriesByCategoryId(int selCategoryId);
    public List<ProductDTO> GetProductByName(string searchString);
    public FilterDTO PopulateFilters();
    public List<ProductDTO> GetFilteredProducts(FilterDTO filter);

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
        var result = _productRepository.GetProductsByCategoryandSubCategory(productCount, categoryId, subCategoryId);
        if (result.Count < 1)
        {
            result = _productRepository.GetProductsByCategoryandSubCategory(productCount, categoryId);
        }
        return result;
    }

    public ProductDTO ProductDetail(int productid)
    {
        return _productRepository.ProductDetail(productid);
    }

    public List<CategoryDTO> GetCategories()
    {
        return _productRepository.GetAllCategories();
    }

    public List<SubCategoryDTO> GetSubCategoriesByCategoryId(int selCategoryId)
    {
        return _productRepository.GetSubCategoriesByCategoryId(selCategoryId);
    }

    public List<ProductDTO> GetProductByName(string searchString)
    {
        return _productRepository.GetProductByName(searchString);
    }

    public FilterDTO PopulateFilters()
    {
        // Filtreleme için gerekli değerler burada doldurulur

        return new FilterDTO
        {
            MinPrice = _productRepository.GetMinPrice(),
            MaxPrice = _productRepository.GetMaxPrice(),
            Colors = _productRepository.GetUniqueColors(),
        };
    }
    public List<ProductDTO> GetFilteredProducts(FilterDTO filter)
    {
        return _productRepository.GetFilteredProducts(

            filter.SelCategoryId,
            filter.SelSubCategoryId,
            filter.SelectedMinPrice,
            filter.SelectedMaxPrice,
            filter.SelectedColors
        );
    }
}
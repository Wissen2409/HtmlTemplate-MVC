
public interface IProductService
{
    public List<ProductDTO> GetProducts
    (
        int productRequested,
        string? selectedSorted = null,
        int? categoryId = 0,
        int? subCategoryId = 0,
        decimal? minPrice = 0,
        decimal? maxPrice = 0,
        List<string>? selectedColors = null
    );
    public ProductDTO ProductDetail(int productid);
    public List<CategoryDTO> GetCategories();
    public List<SubCategoryDTO> GetSubCategoriesByCategoryId(int selCategoryId);
    public List<ProductDTO> GetProductByName(string searchString);
    public FilterDTO PopulateFilters();
}

public class ProductService : IProductService
{
    // servis katmaninda sadece is plani uygulanacagi icin mapleme islemini burda yapmasak daha iyi olur?
    private IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<ProductDTO> GetProducts
    (
        int productRequested,
        string? selectedSorted = null,
        int? categoryId = 0,
        int? subCategoryId = 0,
        decimal? minPrice = 0,
        decimal? maxPrice = 0,
        List<string>? selectedColors = null
    )
    {
        var result = _productRepository.GetProductsByCategoryandSubCategory(productRequested, selectedSorted, categoryId, subCategoryId, minPrice, maxPrice, selectedColors);

        if (result.Count < 1)
        {
            result = _productRepository.GetProductsByCategoryandSubCategory(productRequested, selectedSorted, categoryId);
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
}
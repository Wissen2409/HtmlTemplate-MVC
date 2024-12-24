
public interface IProductService
{
    public List<ProductDTO> GetProducts(int productCount, int categoryId = 0, int subCategoryId = 0);
    public ProductDTO ProductDetail(int productid);
    public List<CategoryDTO> GetCategories();
    public ShopIndexDTO FilterCategoriesAndSubCategories(ShopIndexDTO model);
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
        // eger renk ve ya fiyat secilmisse senin sorgun calismali, secilmediyse benim.
        model.Products = _productRepository.GetProductsByCategoryandSubCategory(12, model.SelCategoryId, model.SelSubCategoryId);
        if (model.Products.Count < 1)
        {
            model.Products = _productRepository.GetProductsByCategoryandSubCategory(12, model.SelCategoryId);
            model.SelSubCategoryId = 0;
        }
        return model;
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
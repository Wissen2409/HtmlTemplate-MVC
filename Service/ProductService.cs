
public interface IProductService
{

    public List<ProductDTO> GetFeatureProduct(int productCount);
    public ProductDTO ProductDetail(int productid);
}
public class ProductService : IProductService
{

    private IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public List<ProductDTO> GetFeatureProduct(int productCount)
    {
        // Repository'den gelen DMO tipini, DTO tipine çevirdik!!

        // istenilirse, automapper kullanılabilir!!
        return _productRepository.GetFeatureProduct(productCount).Select(s => new ProductDTO
        {
            Color = s.Color,
            Id = s.ProductId,
            ListPrice = s.ListPrice,
            ProductName = s.Name,
            StandardCost = s.StandardCost,
        }).ToList();
    }
    public ProductDTO ProductDetail(int productid)
    {

        var returnData = _productRepository.ProductDetail(productid);
        return new ProductDTO
        {
            Color = returnData.Color,
            Description = returnData.ProductLine,
            Id = returnData.ProductId,
            ListPrice = returnData.ListPrice,
            ProductName = returnData.Name,
            StandardCost = returnData.StandardCost,
        };


    }

}
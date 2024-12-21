
public interface IProductService
{
    public List<ProductDTO> GetFeatureProduct(int productCount);
    public ProductDTO ProductDetail(int productid);
    public FilterDTO Filter(FilterDTO model);
}
public class ProductService : IProductService
{
    // servis katmaninda sadece is plani uygulanacagi icin mapleme islemini burda yapmasak daha iyi olur?
    private IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public List<ProductDTO> GetFeatureProduct(int productCount)
    {
        // Repository'den gelen DMO tipini, DTO tipine çevirdik!!

        // istenilirse, automapper kullanılabilir!!
        return _productRepository.GetFeatureProduct(productCount); // direk dto'ya maplenmis olarak geldigi icin hic bir islem yapmamiza gerek kalmadi
    }
    public ProductDTO ProductDetail(int productid)
    {
        return _productRepository.ProductDetail(productid); // alttakilere gerek kalmadi. direk DTO alip gonderdi.

        // var returnData = _productRepository.ProductDetail(productid);

        // new ProductDTO
        // {
        //     Color = returnData.Color,
        //     Description = returnData.ProductLine,
        //     Id = returnData.ProductId,
        //     ListPrice = returnData.ListPrice,
        //     ProductName = returnData.Name,
        //     StandardCost = returnData.StandardCost,
        // };
    }

    public FilterDTO Filter(FilterDTO model)
    {
        if (model.SelCategoryId == 0) // kategory secimi yapilmamissa
        {
            model.Categories = _productRepository.GetAllCategories(); // repodan Category listesini isteyip modelin icine koy
            return model;
        }
        // Kategory id 0 degilse

        model.SubCategories = _productRepository.GetSubCategoriesByCategoryId(model.SelCategoryId);
        // repodan secilmis olan kategory'e ait sub category listesini cekip modelin icine koy

        return model;
    }
}
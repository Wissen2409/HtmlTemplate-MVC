
using AutoMapper;
using HtmlTemplate_MVC.DataAccessLayer;
using HtmlTemplate_MVC.DMO;
using Microsoft.EntityFrameworkCore;

public interface IProductRepository
{
    public List<ProductDTO> GetFeatureProduct(int productCount);
    public ProductDTO ProductDetail(int productid);
}
public class ProductRepository : IProductRepository
{

    private AdventureWorksContext _context;
    private IMapper _mapper;

    public ProductRepository(AdventureWorksContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
    public ProductDTO ProductDetail(int productid)
    {

        // Product tablosundan ProductDescription tablosuna erişmek için, 
        // 3 farklı tabloyu entity framework ile birbirlerine join yapmamız gerekmektedir
        // Cumartesi günü bu join işlemini gerçekleştireceğiz

        string productDescription = _context.ProductDescriptions.Where(s => s.ProductDescriptionId == productid).Select(s => s.Description).FirstOrDefault();


        // alttaki ile ayni problem burda da var(anonim tip),
        // Butun kolonlari cekmek istemiyoruz ayrica DMO da olmayan Description kolonu var, o yuzden sorguyu yaparken gelen yaniti direk DTO olarak alip service katmanina direk DTO olarak yanit gonderiyorm.
        var dtoItem = _context.Products.Where(s => s.ProductId == productid).Select(s => new ProductDTO
        {
            ProductId = s.ProductId,
            Name = s.Name,
            ListPrice = s.ListPrice,
            StandardCost = s.StandardCost,
            Color = s.Color,
            Description = productDescription,

        }).FirstOrDefault();

        return dtoItem;
    }


    public List<ProductDTO> GetFeatureProduct(int productCount)
    {

        // Take ifadesi, sql'deki top ifadesine denk gelmektedir!!

        // Take ifadesi, belirli bir küme üründen sadece belli sayıda getirmeyi isterseniz kullanılabilir

        // Skip ile belirli sayıdaki product'ın listelemeden geçilmesini sağlar!!
        var products = _context.Products
            .Skip(100)
            .Take(productCount)
            .Select(s => new Product
            {
                ProductId = s.ProductId,
                Name = s.Name,
                ListPrice = s.ListPrice,
                Color = s.Color,
                StandardCost = s.StandardCost
            }).ToList();
        // butun kolonlari cekmek yerine sadece DTO icinde ihtiyacimiz olan kolonlari cekiyoruz. Bu yuzden select ile newlerken mecburen maplemek zorundayiz. Anonim tip ile automapper calismiyor...
        // ya da veritabani performansini goz ardi edip direk Product olarak butun kolonlari cekecegiz.


        return _mapper.Map<List<ProductDTO>>(products); // DTO'ya mapleyip donduk.
    }


}
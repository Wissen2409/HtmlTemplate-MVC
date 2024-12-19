
using HtmlTemplate_MVC.DataAccessLayer;
using HtmlTemplate_MVC.DMO;
using Microsoft.EntityFrameworkCore;

public interface IProductRepository
{
    public List<Product> GetFeatureProduct(int productCount);
    public Product ProductDetail(int productid);
}
public class ProductRepository : IProductRepository
{

    private AdventureWorksContext _context;

    public ProductRepository(AdventureWorksContext context)
    {
        _context = context;
    }
    public Product ProductDetail(int productid)
    {

        // Product tablosundan ProductDescription tablosuna erişmek için, 
        // 3 farklı tabloyu entity framework ile birbirlerine join yapmamız gerekmektedir
        // Cumartesi günü bu join işlemini gerçekleştireceğiz

        string productDescription = _context.ProductDescriptions.Where(s => s.ProductDescriptionId == productid).Select(s => s.Description).FirstOrDefault();
        
        
        return _context.Products.Where(s => s.ProductId == productid).Select(s => new Product
        {
            Name = s.Name,
            ListPrice = s.ListPrice,
            Color = s.Color,
            // çakma yöntem itibar etmeyiniz :)  başka bir tablodaki veriyi, dmo içerisindeki boş bir alana yerleştirdim ve öyle modeli geri dönüyorum 
            ProductLine = productDescription

        }).FirstOrDefault();

    }
    public List<Product> GetFeatureProduct(int productCount)
    {

        // Take ifadesi, sql'deki top ifadesine denk gelmektedir!!

        // Take ifadesi, belirli bir küme üründen sadece belli sayıda getirmeyi isterseniz kullanılabilir

        // Skip ile belirli sayıdaki product'ın listelemeden geçilmesini sağlar!!
        return _context.Products.Skip(100).Take(productCount).Select(s => new Product()
        {

            Name = s.Name,
            ListPrice = s.ListPrice,
            Color = s.Color,
            ProductId = s.ProductId,
            StandardCost = s.StandardCost

        }).ToList();
    }


}
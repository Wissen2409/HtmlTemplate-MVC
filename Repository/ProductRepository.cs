
using AutoMapper;
using HtmlTemplate_MVC.DataAccessLayer;
using HtmlTemplate_MVC.DMO;
using Microsoft.EntityFrameworkCore;

public interface IProductRepository
{
    public List<ProductDTO> GetFeatureProduct(int productCount);
    public ProductDTO ProductDetail(int productid);
    public List<CategoryDTO> GetAllCategories();
    public List<SubCategoryDTO> GetSubCategoriesByCategoryId(int categoryId);
    public List<ProductDTO> GetProductsByCategoryandSubCategory(int productRequested, int? categoryId = 0, int? subCategoryId = 0);
    public List<ProductDTO> GetProductByName(string searchString);
    public decimal GetMinPrice();
    public decimal GetMaxPrice();
    public List<string> GetUniqueColors();
    public List<ProductDTO> GetFilteredProducts(int? categoryId, int? subCategoryId, decimal? minPrice, decimal? maxPrice, List<string> selectedColors);

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
        string productDescription = _context.ProductDescriptions.Where(s => s.ProductDescriptionId == productid).Select(s => s.Description).FirstOrDefault();

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

        var products = _context.Products
            .Skip(210)
            .Take(productCount)
            .Select(s => new Product
            {
                ProductId = s.ProductId,
                Name = s.Name,
                ListPrice = s.ListPrice,
                Color = s.Color,
                StandardCost = s.StandardCost
            }).ToList();

        return _mapper.Map<List<ProductDTO>>(products);
    }


    public List<CategoryDTO> GetAllCategories()
    {
        List<ProductCategory> categoriesDMO = _context.ProductCategories.ToList();
        List<CategoryDTO> categoryDTO = _mapper.Map<List<CategoryDTO>>(categoriesDMO);
        return categoryDTO;
    }



    public List<SubCategoryDTO> GetSubCategoriesByCategoryId(int categoryId)
    {
        List<SubCategoryDTO> subCategories = _context.ProductSubcategories
        .Where(x => x.ProductCategoryId == categoryId)
        .Select(y => new SubCategoryDTO
        {
            ProductSubCategoryId = y.ProductSubcategoryId,
            ProductCategoryId = y.ProductCategoryId,
            Name = y.Name
        }).ToList();

        return subCategories;
    }

    /// <summary>
    /// Istenilen miktarda urunleri listeler.
    /// Ayrica category id ve subcategory id verilerek filtreleme yaparak urunleri listeleyebilir.
    /// </summary>
    public List<ProductDTO> GetProductsByCategoryandSubCategory(int productRequested, int? categoryId = 0, int? subCategoryId = 0) // dinamik olarak sorgu yapacak
    {
        var query = _context.Products
        .Include(p => p.ProductProductPhotos)
        .ThenInclude(t => t.ProductPhoto)
        .AsQueryable();
        // tabloya eristik ancak sorguyu henuz gondermedi. Dinamik olarak sorguyu hazirlayip en son gondercem

        if (categoryId.HasValue && categoryId != 0)
        {
            query = query.Where(p => p.ProductSubcategory.ProductCategoryId == categoryId); // sorguya kategory id ye gore filtreleme eklendi
        }

        if (subCategoryId.HasValue && subCategoryId != 0)
        {
            query = query.Where(p => p.ProductSubcategoryId == subCategoryId); // sorguya subcategoryid 'ye gore filtreleme eklendi.
        }

        query = query.Where(p => p.ListPrice > 0).Take(productRequested); // sorguya fiyati olmayan urunlerin cikarilmasi kosulu ve take medodu eklendi

        return query.Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            Name = p.Name,
            ListPrice = p.ListPrice,
            StandardCost = p.StandardCost,
            Color = p.Color,
            LargePhoto = ImgConverter.ConvertImageToBase64(p.ProductProductPhotos.FirstOrDefault().ProductPhoto.LargePhoto),
        }).ToList(); // Sorgu db'ye gonderildi gelen yanit geri donduruldu
    }

    public List<ProductDTO> GetProductByName(string searchString)
    {
       var searchcontext=  _context.Products
       .Where(s => s.Name.Contains(searchString))
       .Where(s => s.ListPrice > 0)
       .Select(s => new ProductDTO
        {   ProductId = s.ProductId,
            Name = s.Name,
            ListPrice = s.ListPrice,
            StandardCost = s.StandardCost,
            Color = s.Color,
            LargePhoto = ImgConverter.ConvertImageToBase64(s.ProductProductPhotos.FirstOrDefault().ProductPhoto.LargePhoto),
        }).ToList(); 
        return searchcontext;
    }


    // Filter By Color ve Filter By Price alanları

    public decimal GetMinPrice()
    {
        return _context.Products.Min(p => p.ListPrice);
    }

    public decimal GetMaxPrice()
    {
        return _context.Products.Max(p => p.ListPrice);
    }

    public List<string> GetUniqueColors()
    {
        return _context.Products
        .Where(p => !string.IsNullOrEmpty(p.Color))
        .Select(p => p.Color)
        .Distinct() // Tekrarlayan elemanları çıkarmak için.
        .ToList();
    }

    public List<ProductDTO> GetFilteredProducts(int? categoryId, int? subCategoryId, decimal? minPrice, decimal? maxPrice, List<string> selectedColors)
    {
        var query = _context.Products
        .Include(p => p.ProductSubcategory)
        .AsQueryable(); // query üzerinde ek filtreleme yapılabilmesini sağlar.

        // Dinamik Filtreleme

        if (categoryId.HasValue && categoryId > 0)
            query = query.Where(p => p.ProductSubcategory.ProductCategoryId == categoryId);
            
        if (subCategoryId.HasValue && subCategoryId > 0)
            query = query.Where(p => p.ProductSubcategoryId == subCategoryId);

        if (minPrice.HasValue)
            query = query.Where(p => p.ListPrice >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.ListPrice <= maxPrice.Value);

        if (selectedColors != null && selectedColors.Any())
            query = query.Where(p => selectedColors.Contains(p.Color));

        return query.Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            Name = p.Name,
            ListPrice = p.ListPrice,
            StandardCost = p.StandardCost,
            Color = p.Color,
        }).ToList();

    }
}

using AutoMapper;
using HtmlTemplate_MVC.DataAccessLayer;
using HtmlTemplate_MVC.DMO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public interface IProductRepository
{
    public List<ProductDTO> GetFeatureProduct(int productCount);
    public ProductDTO ProductDetail(int productid);
    public List<CategoryDTO> GetAllCategories();
    public List<SubCategoryDTO> GetSubCategoriesByCategoryId(int categoryId);
    public List<ProductDTO> GetProductsByCategoryandSubCategory
    (
        int productRequested,
        string? selectedSorted = null,
        int? categoryId = 0,
        int? subCategoryId = 0,
        decimal? minPrice = 0,
        decimal? maxPrice = 0,
        List<string>? selectedColors = null
    );
    public List<ProductDTO> GetProductByName(string searchString);
    public decimal GetMinPrice();
    public decimal GetMaxPrice();
    public List<string> GetUniqueColors();
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
    public List<ProductDTO> GetProductsByCategoryandSubCategory // dinamik olarak sorgu yapacak
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
        var query = _context.Products
        .Include(p => p.ProductProductPhotos)
        .ThenInclude(t => t.ProductPhoto)
        .AsQueryable();

        #region kategory ve alt kategory sorgusu
        if (categoryId.HasValue && categoryId != 0)
        {
            query = query.Where(p => p.ProductSubcategory.ProductCategoryId == categoryId); // sorguya kategory id ye gore filtreleme eklendi

            if (subCategoryId.HasValue && subCategoryId != 0)
            {
                query = query.Where(p => p.ProductSubcategoryId == subCategoryId); // sorguya subcategoryid 'ye gore filtreleme eklendi.
            }
        }
        #endregion

        #region  renk ve fiyat filtreleri sorgusu
        if (minPrice.HasValue)
            query = query.Where(p => p.StandardCost >= minPrice.Value);

        if (maxPrice.HasValue && minPrice > 0)
            query = query.Where(p => p.StandardCost <= maxPrice.Value);

        if (selectedColors != null && selectedColors.Any())
            query = query.Where(p => selectedColors.Contains(p.Color));
        #endregion

        #region  siralama islemi sorguya eklendi (son adim)
        // Sorted seçildiyse ilgili işlem burada databaseden çekilmeden yapılması gerekiyor. Ürün adeti sorun çıkarmaması için where koşulundan önce eklendi ;
        switch (selectedSorted)
        {
            case "PriceAsc":
                query = query.OrderBy(x => x.StandardCost);
                break;
            case "PriceDesc":
                query = query.OrderByDescending(x => x.StandardCost);
                break;
            case "NameAsc":
                query = query.OrderBy(x => x.Name);
                break;
            case "NameDesc":
                query = query.OrderByDescending(x => x.Name);
                break;
        }
        #endregion

        query = query.Take(productRequested);
        return query.Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            Name = p.Name,
            ListPrice = p.ListPrice,
            StandardCost = p.StandardCost,
            Color = p.Color,
            LargePhoto = ImgConverter.ConvertImageToBase64(p.ProductProductPhotos.FirstOrDefault().ProductPhoto.LargePhoto),
        }).ToList();
    }



    public List<ProductDTO> GetProductByName(string searchString)
    {
        var searchcontext = _context.Products
        .Where(s => s.Name.Contains(searchString))
        .Where(s => s.ListPrice > 0)
        .Select(s => new ProductDTO
        {
            ProductId = s.ProductId,
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

}
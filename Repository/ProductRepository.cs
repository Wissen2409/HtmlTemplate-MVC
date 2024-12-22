
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
    public List<ProductDTO> GetProductsByCategoryandSubCategory(int? categoryId, int? subCategoryId);
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


    public List<ProductDTO> GetProductsByCategoryandSubCategory(int? categoryId, int? subCategoryId) // dinamik olarak sorgu yapacak
    {
        var query = _context.Products.AsQueryable(); // tabloya eristik ancak sorguyu henuz gondermedi. Dinamik olarak sorguyu hazirlayip en son gondercem

        if (categoryId != 0)
        {
            query = query.Where(p => p.ProductSubcategory.ProductCategoryId == categoryId);
        }

        if (subCategoryId != 0)
        {
            query = query.Where(p => p.ProductSubcategoryId == subCategoryId);
        }

        return query.Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            Name = p.Name,
            ListPrice = p.ListPrice,
            StandardCost = p.StandardCost,
            Color = p.Color
        }).ToList();


    }
}
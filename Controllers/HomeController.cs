using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HtmlTemplate_MVC.Models;
using AutoMapper;

namespace HtmlTemplate_MVC.Controllers;

public class HomeController : Controller
{
    private IMapper _mapper;
    private IProductService _productService;
    private IProductCategoryService _productCategoryService;
    public HomeController(IProductService productService, IMapper mapper,IProductCategoryService productCategoryService)
    {
        _mapper = mapper;
        _productService = productService;
        _productCategoryService = productCategoryService;
    }

    public IActionResult Index()
    {

        // View modeli initalize edelim 
        IndexViewModel model = new IndexViewModel();

        var dtoFeaturedList = _productService.GetProducts(12); // servisten yanit DTO olarak geldi.
        var featuredProducts = _mapper.Map<List<ProductViewModel>>(dtoFeaturedList); // DTO listesini VM listesine donusturur.
        model.Products = featuredProducts; // modelin icine listeyi koyk

        var categoryList = _mapper.Map<List<CategoryVM>>(_productCategoryService.GetAll());
        model.Categories = categoryList;

        return View(model);
    }


    public IActionResult ProductDetail(int productId)
    {

        return View(_mapper.Map<ProductViewModel>(_productService.ProductDetail(productId))); // servisten aldigin DTO yaniti automapper ile VM'e cevirip View'a model olarak yolla

        // var returnData = _productService.ProductDetail(productId);

        // return View(new ProductViewModel
        // {
        //     Color = returnData.Color,
        //     Description = returnData.Description,
        //     Id = returnData.Id,
        //     ListPrice = returnData.ListPrice,
        //     Name = returnData.ProductName,
        //     StandardCost = returnData.StandardCost
        // });
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HtmlTemplate_MVC.Models;
using AutoMapper;

namespace HtmlTemplate_MVC.Controllers;

public class HomeController : Controller
{
    private IMapper _mapper;
    private IProductService _productService;
    public HomeController(IProductService productService, IMapper mapper)
    {
        _mapper = mapper;
        _productService = productService;
    }

    public IActionResult Index()
    {

        // View modeli initalize edelim 
        IndexViewModel model = new IndexViewModel();

        var dtoFeaturedList = _productService.GetProducts(12); // servisten yanit DTO olarak geldi.
        var featuredProducts = _mapper.Map<List<ProductViewModel>>(dtoFeaturedList); // DTO listesini VM listesine donusturur.
        model.Products = featuredProducts; // modelin icine listeyi koyk

        var categoryList = _mapper.Map<List<CategoryVM>>(_productService.GetCategories());
        model.Categories = categoryList;

        return View(model);
    }

    public IActionResult ProductDetail(int productId)
    {
        return View(_mapper.Map<ProductViewModel>(_productService.ProductDetail(productId)));
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

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HtmlTemplate_MVC.Models;

namespace HtmlTemplate_MVC.Controllers;

public class HomeController : Controller
{

    private IProductService _productService;
    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {

        // View modeli initalize edelim 

        IndexViewModel model = new IndexViewModel();


        model.Products = _productService.GetFeatureProduct(120).Select(s => new ProductViewModel
        {

            Color = s.Color,
            Id = s.Id,
            ListPrice = s.ListPrice,
            Name = s.ProductName,
            StandardCost = s.StandardCost
        }).ToList();


        return View(model);
    }
    public IActionResult ProductDetail(int productId)
    {

        var returnData = _productService.ProductDetail(productId);

        return View(new ProductViewModel
        {
            Color = returnData.Color,
            Description = returnData.Description,
            Id = returnData.Id,
            ListPrice = returnData.ListPrice,
            Name = returnData.ProductName,
            StandardCost = returnData.StandardCost
        });
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

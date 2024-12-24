using AutoMapper;
using HtmlTemplate_MVC.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;


public class SearchController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    public SearchController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }


    [HttpPost]
    public IActionResult Search(string searchstring)
    {

        // ViewModel'i gönder
        return RedirectToAction("Index", "Shop",new { searchString = searchstring }); // ShopController'daki Index'e yönlendir
    }
}
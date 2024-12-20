using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class ShopController : Controller
{
    private IMapper _mapper;
    public ShopController(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View();
    }
}
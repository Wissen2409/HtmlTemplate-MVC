using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class ShopController : Controller
{
    private IMapper _mapper;
    private IProductService _service;
    public ShopController(IMapper mapper, IProductService service)
    {
        _mapper = mapper;
        _service = service;
    }

    public IActionResult Index()
    {
        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData,
            ("Home", "/"),
            ("Shop", null)
            );

        ShopIndexVM model = new();
        model.Products = _mapper.Map<List<ProductViewModel>>(_service.GetProducts(9));
        model.Categories = _mapper.Map<List<CategoryVM>>(_service.GetCategories());
        return View(model);
    }
    [HttpPost]
    public IActionResult Index(ShopIndexVM model)
    {
        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData,
            ("Home", "/"),
            ("Shop", null)
            );

        var dtoModel = _service.FilterCategoriesAndSubCategories(_mapper.Map<ShopIndexDTO>(model));
        return View(_mapper.Map<ShopIndexVM>(dtoModel));
    }
}
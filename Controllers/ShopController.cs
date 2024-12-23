using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class ShopController : Controller
{
    private IMapper _mapper;
    private IProductService _service;
    public ShopController(IMapper mapper, IProductService service)
    {
        _mapper = mapper;
        _service = service;
    }


    public IActionResult Index(int selectedID = 0)
    {
        if (selectedID == 0)
        {
            BreadCrumbViewBagHelper.SetBreadCrumb(ViewData,
                ("Home", "/"),
                ("Shop", null)
                );

            ShopIndexVM model = new();

            model.Products = _mapper.Map<List<ProductViewModel>>(_service.GetProducts(9));
            model.Categories = _mapper.Map<List<CategoryVM>>(_service.GetCategories());
        
            var filters = _service.PopulateFilters();
            model.MinPrice = filters.MinPrice;
            model.MaxPrice = filters.MaxPrice;
            model.Colors = filters.Colors;

            return View(model);
        }
        else
        {
            ShopIndexVM model = new ShopIndexVM
            {
                SelCategoryId = selectedID
            };
            var dtoModel = _service.FilterCategoriesAndSubCategories(_mapper.Map<ShopIndexDTO>(model));
            return View(_mapper.Map<ShopIndexVM>(dtoModel));

        }

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

    public IActionResult Click(int selectedCategoryID)
    {
        // Form gönderimi için Index metoduna yönlendirme yapıyoruz
        return RedirectToAction("Index", "Shop", new { selectedID = selectedCategoryID });
    }
}


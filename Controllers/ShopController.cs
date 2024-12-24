using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class ShopController : Controller
{
    private IMapper _mapper;
    private IProductService _service;
    private const int pageSize = 12; //her sayfada gosterilecek urun sayisi
    public ShopController(IMapper mapper, IProductService service)
    {
        _mapper = mapper;
        _service = service;
    }


    public IActionResult Index(int selectedCatID = 0)
    {

        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData, ("Home", "/"), ("Shop", null));

        if (selectedCatID == 0)
        {
            ShopIndexVM createdModel = new()
            {
                CurrentPage = 1,
            };
            var dtoModel = _service.FilterCategoriesAndSubCategories(_mapper.Map<ShopIndexDTO>(createdModel), pageSize);

            var model = _mapper.Map<ShopIndexVM>(dtoModel);

            return View(model);
        }
        else
        {
            ShopIndexVM model = new ShopIndexVM
            {
                SelCategoryId = selectedCatID,
                CurrentPage = 1,
            };
            var dtoModel = _service.FilterCategoriesAndSubCategories(_mapper.Map<ShopIndexDTO>(model), pageSize);
            return View(_mapper.Map<ShopIndexVM>(dtoModel));
        }


    }

    [HttpPost]
    public IActionResult Index(ShopIndexVM model)
    {

        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData, ("Home", "/"), ("Shop", null));
        if (model.CurrentPage < 1)
        {
            model.CurrentPage = 1;
        }
        model.SkipCount = (model.CurrentPage - 1) * pageSize;
        var dtoModel = _service.FilterCategoriesAndSubCategories(_mapper.Map<ShopIndexDTO>(model), pageSize);
        return View(_mapper.Map<ShopIndexVM>(dtoModel));
    }

    public IActionResult Click(int selectedCategoryID)
    {
        // Form gönderimi için Index metoduna yönlendirme yapıyoruz
        return RedirectToAction("Index", "Shop", new { selectedCatID = selectedCategoryID });
    }

}
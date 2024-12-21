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
        ShopIndexVM model = new();
        model.Products = _mapper.Map<List<ProductViewModel>>(_service.GetFeatureProduct(9));
        model.Filters = _mapper.Map<FilterVM>(_service.Filter(new FilterDTO()));

        return View(model);
    }
    [HttpPost]
    public IActionResult Index(FilterVM filterModel)
    {
        ShopIndexVM model = new();

        var dtoFilterAnswer = _service.Filter(_mapper.Map<FilterDTO>(filterModel)); // gelen filtermoldei service katmanina at
        var returnFilter = _mapper.Map<FilterVM>(dtoFilterAnswer); // gelen yaniti VM e cevir
        model.Filters = returnFilter; // modele ekle

        if (model.Filters.SelCategoryId != 0 && model.Filters.SelSubCategoryId != 0)
        {
            // eger hem category hem de subcategory secildiyse, urunleri filtreleyip getirmek lazim...
        }
        model.Products = _mapper.Map<List<ProductViewModel>>(_service.GetFeatureProduct(9)); // yoksa ilk 9 urunu getir


        return View(model);
    }
}
using AutoMapper;
using HtmlTemplate_MVC.DataAccessLayer;
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



    public IActionResult Index(int selCategoryId = 0, int selSubCategoryId = 0, string searchString = "", string selectedSorted = null, decimal? selectedMinPrice = null, decimal? selectedMaxPrice = null, List<string> selectedColors = null)
    {
        BreadCrumbViewBagHelper.SetBreadCrumb(ViewData, ("Home", "/"), ("Shop", null)); // Breadcrumb icin gerekli

        ShopIndexVM model = new ShopIndexVM();                                          // sayfaya gonderilecek olan model

        model.Categories = _mapper.Map<List<CategoryVM>>(_service.GetCategories());     // kategoriler listesi modele her zaman konmali

        model.SelCategoryId = selCategoryId;                                            // category ve subcategor secimi modele yerlestirilmeli
        model.SelSubCategoryId = selSubCategoryId;
        TempData["SelectedCategoryID"] = model.SelCategoryId;
        TempData["SelectedSubCategoryID"] = model.SelSubCategoryId;

        if (model.SelCategoryId > 0)                                 // eger kategory secimi yapilmissa, subcategories listesi modele eklenmeli
        {
            model.SubCategories = _mapper.Map<List<SubCategoryVM>>(_service.GetSubCategoriesByCategoryId(model.SelCategoryId));
        }

        var filters = _service.PopulateFilters();                                       // Filtreler her modelde olmali
        model.MinPrice = filters.MinPrice;
        model.MaxPrice = filters.MaxPrice;
        model.Colors = filters.Colors;

        // Eğer filtreleme yapılmissa

        if (selectedMinPrice.HasValue || selectedMaxPrice.HasValue || (selectedColors != null && selectedColors.Any()))
        {

            var filterDTO = new FilterDTO
            {

                SelCategoryId = selCategoryId,
                SelSubCategoryId = selSubCategoryId,
                SelectedMinPrice = selectedMinPrice,
                SelectedMaxPrice = selectedMaxPrice,
                SelectedColors = selectedColors
            };

            var filteredProducts = _service.GetFilteredProducts(filterDTO);
            model.Products = _mapper.Map<List<ProductViewModel>>(filteredProducts);
        }
        else if (!string.IsNullOrEmpty(searchString))                                        // Arama metni varsa
        {                                                                           // product listesi arama yaparak olusturulmali
            var dtoResult = _service.GetProductByName(searchString);
            var result = _mapper.Map<List<ProductViewModel>>(dtoResult);

            model.Products = result;
        }

        else
        {
            // category ve subcategory secimi yapilmissa sorgu ona gore gelecek zaten, Ayri bir kosul belitrmeye gerek yok. Repo katmaninda sorgu her 3 duruma uygun olacak sekilde yazildi.
            model.Products = _mapper.Map<List<ProductViewModel>>(_service.GetProducts(9, model.SelCategoryId, model.SelSubCategoryId, selectedSorted));
        }


        return View(model);
    }

    [HttpPost]
    public IActionResult Index(ShopIndexVM model) // mofdelde gelen category idleri bilgisini index actionuna parametre olarak gonderecek
    {
        return RedirectToAction("Index", "Shop", new
        {
            selCategoryId = model.SelCategoryId,
            selSubCategoryId = model.SelSubCategoryId,
            selectedMinPrice = model.SelectedMinPrice,
            selectedMaxPrice = model.SelectedMaxPrice,
            selectedColors = model.SelectedColors
        });
    }

    public IActionResult SelectCategory(int selectedCategoryID)
    {
        // Form gönderimi için Index metoduna yönlendirme yapıyoruz
        return RedirectToAction("Index", "Shop", new { selCategoryId = selectedCategoryID });
    }
    public IActionResult SelectSorted(string selectedSorted)
    {
        var selectedCategoryID = TempData["SelectedCategoryID"];
        var selectedSubCategoryID = TempData["SelectedSubCategoryID"];
        TempData["selectedSorted"] = selectedSorted;


        return RedirectToAction("Index", "Shop", new { selectedSorted = selectedSorted, selCategoryID = selectedCategoryID, selSubCategoryId = selectedSubCategoryID });
    }
}


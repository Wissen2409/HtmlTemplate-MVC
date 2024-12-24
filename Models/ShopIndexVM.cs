public class ShopIndexVM
{
    public List<ProductViewModel> Products { get; set; }
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryVM> Categories { get; set; }
    public List<SubCategoryVM> SubCategories { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int SkipCount { get; set; }
}
public class ShopIndexVM
{
    public List<ProductViewModel> Products { get; set; }
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryVM> Categories { get; set; }
    public List<SubCategoryVM> SubCategories { get; set; }

    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public List<string> Colors { get; set; }

    public decimal? SelectedMinPrice { get; set; }
    public decimal? SelectedMaxPrice { get; set; }
    public List<string> SelectedColors { get; set; }

}
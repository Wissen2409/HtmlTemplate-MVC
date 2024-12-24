public class FilterVM
{
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryVM> Categories { get; set; }
    public List<SubCategoryVM> SubCategories { get; set; }
    public int CurrentPage { get; set; }
}




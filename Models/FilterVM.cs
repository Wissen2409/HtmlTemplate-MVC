public class FilterVM
{
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryVM> Categories { get; set; }
    public List<SubCategoryVM> SubCategories { get; set; }
}

public class CategoryVM
{
    public int ProductCategoryId { get; set; }
    public string Name { get; set; }
}

public class SubCategoryVM
{
    public int ProductCategoryId { get; set; }
    public int ProductSubCategoryId { get; set; }
    public string Name { get; set; }
}
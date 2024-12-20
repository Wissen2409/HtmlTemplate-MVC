public class FilterDTO
{
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryDTO> Categories { get; set; }
    public List<SubCategoryDTO> SubCategories { get; set; }
}

public class CategoryDTO
{
    public int ProductCategoryId { get; set; }
    public string Name { get; set; }
}

public class SubCategoryDTO
{
    public int ProductCategoryId { get; set; }
    public int ProductSubCategoryId { get; set; }
    public string Name { get; set; }
}
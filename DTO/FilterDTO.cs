public class FilterDTO
{
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryDTO> Categories { get; set; }
    public List<SubCategoryDTO> SubCategories { get; set; }
}



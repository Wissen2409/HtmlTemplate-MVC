public class ShopIndexDTO
{
    public List<ProductDTO> Products { get; set; }
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryDTO> Categories { get; set; }
    public List<SubCategoryDTO> SubCategories { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int SkipCount { get; set; }
}




public class ShopIndexDTO
{
    public List<ProductDTO> Products { get; set; }
    public int SelCategoryId { get; set; }
    public int SelSubCategoryId { get; set; }
    public List<CategoryDTO> Categories { get; set; }
    public List<SubCategoryDTO> SubCategories { get; set; }

     public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public List<string> Colors { get; set; }
}




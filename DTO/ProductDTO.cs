public class ProductDTO : DTOBase
{

    public string ProductName { get; set; }
    public decimal ListPrice { get; set; }
    public string Color { get; set; }
    
    public decimal StandardCost { get; set; }

    public string Description { get; set; }

}
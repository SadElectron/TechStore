namespace TechStore.Api.Models.Product;

public class UpdateProductModel
{
    public Guid Id { get; set; }
    public required string ProductName { get; set; }
    public int Stock { get; set; }
    public int SoldQuantity { get; set; }
    public double Price { get; set; }
}


namespace TechStore.Api.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public int ProductOrder { get; set; }
    public required string ProductName { get; set; }
    public int Stock { get; set; }
    public int SoldQuantity { get; set; }
    public double Price { get; set; }
    public DateTime LastUpdate { get; set; }
}

using System.ComponentModel.DataAnnotations;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Product;

public class CreateProductModel : IValidationModel
{
    public required Guid CategoryId { get; set; }
    public required string ProductName { get; set; } = string.Empty;
    public required int Stock { get; set; }
    public required double Price { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace TechStore.Api.Models
{
    public class CreateProductModel
    {
        public required Guid CategoryId { get; set; }
        public required string ProductName { get; set; } = string.Empty;
        public required int Stock { get; set; }
        public required double Price { get; set; }
    }
}

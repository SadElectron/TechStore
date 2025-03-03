using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Product;

public class DeleteProductModel
{
    [FromRoute(Name = "productId")]
    public Guid Id { get; set; }
}

using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Product;

public class ProductIdModel
{
    [FromRoute(Name = "productId")]
    public Guid Id { get; set; }
}
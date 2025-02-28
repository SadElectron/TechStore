using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Product;

public class ProductPageModel
{
    [FromRoute(Name = "page")]
    public int Page { get; set; }
    [FromRoute(Name = "count")]
    public int Count { get; set; }
}

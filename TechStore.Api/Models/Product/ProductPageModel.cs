using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Product;

public class ProductPageModel : IValidationModel
{
    [FromRoute(Name = "page")]
    public int Page { get; set; }
    [FromRoute(Name = "count")]
    public int Count { get; set; }
}

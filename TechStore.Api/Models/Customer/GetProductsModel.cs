using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Customer;

public class GetProductsModel : IValidationModel
{
    [FromRoute(Name = "categoryId")]
    public Guid CategoryId { get; set; }

    [FromRoute(Name = "page")]
    public int Page { get; set; }

    [FromRoute(Name = "itemCount")]
    public int ItemCount { get; set; }
}

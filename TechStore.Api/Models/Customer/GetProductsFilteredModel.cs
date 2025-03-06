using Core.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Customer;

public class GetProductsFilteredModel : IValidationModel
{
    [FromRoute(Name = "categoryId")]
    public Guid CategoryId { get; set; }

    [FromRoute(Name = "page")]
    public int Page { get; set; }

    [FromRoute(Name = "itemCount")]
    public int ItemCount { get; set; }

    [FromBody]
    public List<ProductFilterModel> Filters { get; set; } = new List<ProductFilterModel>();
}

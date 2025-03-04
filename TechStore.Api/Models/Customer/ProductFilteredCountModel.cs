using Core.RequestModels;
using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Customer;

public class ProductFilteredCountModel : IValidationModel
{
    [FromRoute]
    public Guid CategoryId { get; set; }

    [FromBody]
    public List<ProductFilterModel> Filters { get; set; } = new List<ProductFilterModel>();
}

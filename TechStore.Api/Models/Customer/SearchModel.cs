using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Customer;

public class SearchModel : IValidationModel
{
    [FromQuery]
    public string Query { get; set; } = string.Empty;
}
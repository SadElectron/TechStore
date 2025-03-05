using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Customer;

public class SearchModel
{
    [FromQuery]
    public string Query { get; set; } = string.Empty;
}
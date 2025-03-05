using Core.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Customer;
//[FromBody] FilterAndSortModel filterAndSort,
//Guid categoryId = default,
//int page = 1,
//int itemCount = 10
public class GetProductsFilteredAndSortedModel
{
    [FromRoute(Name = "categoryId")]
    public Guid CategoryId { get; set; }
    
    [FromRoute(Name = "page")]
    public int Page { get; set; }

    [FromRoute(Name = "itemCount")]
    public int ItemCount { get; set; }

    [FromBody]
    public FilterAndSortModel FilterAndSort { get; set; } = new FilterAndSortModel();

}

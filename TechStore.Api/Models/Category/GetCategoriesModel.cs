using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Category;

public class GetCategoriesModel
{
    [FromRoute(Name = "page")]
    public int Page { get; set; }

    [FromRoute(Name = "itemCount")]
    public int Count { get; set; }

}

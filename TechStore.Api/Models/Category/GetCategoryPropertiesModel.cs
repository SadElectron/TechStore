using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Category;
public class GetCategoryPropertiesModel
{
    [FromRoute(Name = "categoryId")]
    public Guid CategoryId { get; set; }

    [FromRoute(Name = "page")]
    public int Page { get; set; }

    [FromRoute(Name = "count")]
    public int Count { get; set; }

}

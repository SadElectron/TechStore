using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Category;

public class CategoryIdModel
{
    [FromRoute(Name = "categoryId")]
    public Guid Id { get; set; }
}

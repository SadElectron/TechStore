using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Category;

public class CategoryIdModel : IValidationModel
{
    [FromRoute(Name = "categoryId")]
    public Guid Id { get; set; }
}

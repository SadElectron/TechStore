using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Product;

public class DeleteProductModel : IValidationModel
{
    [FromRoute(Name = "productId")]
    public Guid Id { get; set; }
}

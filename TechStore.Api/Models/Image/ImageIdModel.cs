using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Image;

public class ImageIdModel : IValidationModel
{
    [FromRoute(Name = "imageId")]
    public Guid Id { get; set; }
}

using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Image;

public class ImageIdModel
{
    [FromRoute(Name = "imageId")]
    public Guid Id { get; set; }
}

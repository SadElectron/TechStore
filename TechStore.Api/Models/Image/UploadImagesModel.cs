namespace TechStore.Api.Models.Image;

public class UploadImagesModel
{
    public Guid ProductId { get; set; }
    public required List<IFormFile> Files { get; set; } 
}

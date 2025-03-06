using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Image;

public class UploadImagesModel : IValidationModel
{
    public Guid ProductId { get; set; }
    public required List<IFormFile> Files { get; set; } 
}

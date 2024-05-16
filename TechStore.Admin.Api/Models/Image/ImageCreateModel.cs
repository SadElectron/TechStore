namespace TechStore.Areas.Admin.Models.Image
{
    public record class ImageCreateModel(Guid ProductId, bool ImageType, string ProductType);
}

namespace TechStore.Areas.Admin.Models.Image
{
    public class ImageUpdateModel
    {
        public Guid ProductId { get; set; }
        public string ProductType{ get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public ICollection<Entities.Concrete.Image> Images { get; set; } = new List<Entities.Concrete.Image>();

    }
}

namespace TechStore.Areas.Admin.Models.Image
{
    public class ImageCpuModel
    {
        public Guid CpuId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public ICollection<Entities.Concrete.Image> Images { get; set; } = new List<Entities.Concrete.Image>();
        public override string ToString()
        {
            return $"{Brand} {ModelName}";
        }
    }
}

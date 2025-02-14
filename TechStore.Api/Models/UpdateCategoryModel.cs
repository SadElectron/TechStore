namespace TechStore.Api.Models
{
    public class UpdateCategoryModel
    {
        public Guid Id { get; set; }
        public double RowOrder { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}

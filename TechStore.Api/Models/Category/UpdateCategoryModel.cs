namespace TechStore.Api.Models.Category
{
    public class UpdateCategoryModel
    {
        public Guid Id { get; set; }
        public double RowOrder { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}

using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Category
{
    public class UpdateCategoryModel : IValidationModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}

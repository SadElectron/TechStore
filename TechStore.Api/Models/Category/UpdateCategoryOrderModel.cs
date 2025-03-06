using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Category;

public class UpdateCategoryOrderModel : IValidationModel
{
    public Guid Id { get; set; }
    public double RowOrder { get; set; }
}
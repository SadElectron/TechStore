using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Product;

public class UpdateProductOrderModel : IValidationModel
{
    public Guid Id { get; set; }
    public double ProductOrder { get; set; }
}
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Image;

public class UpdateImageOrderModel : IValidationModel
{
    public Guid Id { get; set; }
    public double NewOrder { get; set; }
}
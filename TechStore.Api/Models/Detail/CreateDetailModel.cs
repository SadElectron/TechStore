using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Detail;

public class CreateDetailModel : IValidationModel
{
    public required string PropValue { get; set; }
    public Guid PropertyId { get; set; }
    public Guid ProductId { get; set; }
}

using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Detail;

public class UpdateDetailModel : IValidationModel
{
    public Guid Id { get; set; }
    public required string PropValue { get; set; }
}

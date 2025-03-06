using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Property;

public class CreatePropertyModel : IValidationModel
{
    public required string PropName { get; set; }
    public Guid CategoryId { get; set; }
}

using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Property;

public class UpdatePropertyModel : IValidationModel
{
    public Guid Id { get; set; }
    public required string PropName { get; set; }
}

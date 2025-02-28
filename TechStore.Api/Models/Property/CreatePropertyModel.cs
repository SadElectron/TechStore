namespace TechStore.Api.Models.Property;

public class CreatePropertyModel
{
    public required string PropName { get; set; }
    public Guid CategoryId { get; set; }
}

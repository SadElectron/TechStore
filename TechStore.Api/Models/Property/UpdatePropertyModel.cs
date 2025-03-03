namespace TechStore.Api.Models.Property;

public class UpdatePropertyModel
{
    public Guid Id { get; set; }
    public required string PropName { get; set; }
}

namespace TechStore.Api.Models;

public class CreateDetailModel
{
    public required string PropValue { get; set; }
    public Guid PropertyId { get; set; }
    public Guid ProductId { get; set; }
}

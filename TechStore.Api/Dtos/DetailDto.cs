namespace TechStore.Api.Dtos;

public class DetailDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public double RowOrder { get; set; }
    public required string PropName { get; set; }
    public required string PropValue { get; set; }
    public DateTime LastUpdate { get; set; }
}

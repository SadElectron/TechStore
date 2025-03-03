using Core.Entities.Concrete;

namespace TechStore.Api.Dtos;

public class PropertyDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public int PropOrder { get; set; }
    public required string PropName { get; set; }
    public DateTime LastUpdate { get; set; }

}

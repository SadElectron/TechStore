using Core.Entities.Concrete;

namespace Core.Dtos;

public class PropertyDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public int PropOrder { get; set; }
    public required string PropName { get; set; }
    public DateTime LastUpdate { get; set; }

}

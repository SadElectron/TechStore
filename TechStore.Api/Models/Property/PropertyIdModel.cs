using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Property;

public class PropertyIdModel
{
    [FromRoute(Name = "propertyId")]
    public Guid Id { get; set; }
}

using Microsoft.AspNetCore.Mvc;

namespace TechStore.Api.Models.Property
{
    public class UpdatePropOrderModel
    {
        [FromRoute(Name = "propertyId")]
        public Guid Id { get; set; }
        public double PropOrder { get; set; }
    }
}

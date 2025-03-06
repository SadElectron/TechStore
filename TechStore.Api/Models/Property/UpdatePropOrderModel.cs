using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Property
{
    public class UpdatePropOrderModel : IValidationModel
    {
        [FromRoute(Name = "propertyId")]
        public Guid Id { get; set; }
        public double PropOrder { get; set; }
    }
}

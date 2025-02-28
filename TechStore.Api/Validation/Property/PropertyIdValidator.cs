using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Property;

namespace TechStore.Api.Validation.Property;

public class PropertyIdValidator : AbstractValidator<PropertyIdModel>
{
    public PropertyIdValidator(IPropertyService propertyService)
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Property id is required.")
            .NotEqual(Guid.Empty).WithMessage("Property id cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await propertyService.ExistsAsync(id)).WithMessage("Property id is not valid.");
    }
}

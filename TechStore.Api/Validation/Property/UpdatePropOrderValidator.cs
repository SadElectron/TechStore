using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Property;

namespace TechStore.Api.Validation.Property
{
    public class UpdatePropOrderValidator : AbstractValidator<UpdatePropOrderModel>
    {
        public UpdatePropOrderValidator(IPropertyService propertyService)
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Property id is required.")
                .NotEqual(Guid.Empty).WithMessage("Property id cannot be empty.")
                .MustAsync(async (id, cancellationToken) => await propertyService.ExistsAsync(id)).WithMessage("Property id is not valid.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PropOrder)
                        .Cascade(CascadeMode.Stop)
                        .NotEmpty().WithMessage("Property order is required.")
                        .GreaterThan(0).WithMessage("Property order must be greater than 0.")
                        .MustAsync(async (model, order, cancellationToken) => order <= await propertyService.GetLastPropOrderByPropertyIdAsync(model.Id))
                        .WithMessage("New order must be less than or equal to the last property order.");
                    
                });
        }
    }
}

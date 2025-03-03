using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Property;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Validation.Property;

public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyModel>
{
    public UpdatePropertyValidator(IPropertyService propertyService)
    {
        RuleFor(x => x.Id)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("Property id is required.")
               .NotEqual(Guid.Empty).WithMessage("Property id cannot be empty.")
               .MustAsync(async (id, cancellationToken) => await propertyService.ExistsAsync(id)).WithMessage("Property id is not valid.")
        .DependentRules(() => {
             RuleFor(x => x.PropName)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty().WithMessage("Property name is required.")
                 .Equal(x => x.PropName.Trim()).WithMessage((model) => $"{model.PropName} cannot have leading or trailing spaces.")
                 .MinimumLength(2).WithMessage((model) => $"{model.PropName} must be at least 2 characters long.")
                 .MaximumLength(50).WithMessage((model) => $"{model.PropName} cannot be longer than 50 characters.")
                 .Matches(@"^[a-zA-Z0-9]+(?:\s[a-zA-Z0-9]+)*$")
                 .WithMessage((model) => $"{model.PropName} can only contain alphanumeric characters and spaces.")
                 .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x))
                 .WithMessage((model) => $"{model.PropName} contains invalid characters such as '--', single quotes, or semicolons.");
         });
    }
}

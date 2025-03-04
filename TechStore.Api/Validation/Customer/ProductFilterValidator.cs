using Core.RequestModels;
using FluentValidation;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Validation.Customer;

public class ProductFilterValidator : AbstractValidator<ProductFilterModel>
{
    public ProductFilterValidator()
    {
        RuleFor(x => x.FilterKey)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("FilterKey is required.")
            .Equal(x => x.FilterKey.Trim()).WithMessage("FilterKey cannot have leading or trailing spaces.")
            .MinimumLength(3).WithMessage("FilterKey must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("FilterKey cannot be longer than 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-.\/]+(?:\s[a-zA-Z0-9\-.\/]+)*$").WithMessage("FilterKey can only contain alphanumeric characters, spaces, hyphens, periods, and forward slashes.")
            .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("FilterKey contains invalid characters such as '--', single quotes, or semicolons.");
        RuleForEach(x => x.FilterValues).ChildRules(fv =>
        {
            fv.RuleFor(fv => fv).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("FilterValue is empty.")
                .Equal(x => x.Trim()).WithMessage("{PropertyValue} cannot have leading or trailing spaces.")
                .MinimumLength(3).WithMessage("{PropertyValue} must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("{PropertyValue} cannot be longer than 50 characters.")
                .Matches(@"^[a-zA-Z0-9\-.\/]+(?:\s[a-zA-Z0-9\-.\/]+)*$").WithMessage("{PropertyValue} can only contain alphanumeric characters, spaces, hyphens, periods, and forward slashes.")
                .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("{PropertyValue} contains invalid characters such as '--', single quotes, or semicolons.");
        });
            
    }
}

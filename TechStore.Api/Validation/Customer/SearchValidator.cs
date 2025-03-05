namespace TechStore.Api.Validation.Customer;
using FluentValidation;
using TechStore.Api.Models.Customer;
using TechStore.Api.Validation.Utils;

public class SearchValidator : AbstractValidator<SearchModel> 
{
    public SearchValidator()
    {
        RuleFor(x => x.Query)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Equal(x => x.Query.Trim()).WithMessage("{PropertyValue} cannot have leading or trailing spaces.")
            .MinimumLength(3).WithMessage("{PropertyValue} must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("{PropertyValue} cannot be longer than 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-.\/]+(?:\s[a-zA-Z0-9\-.\/]+)*$").WithMessage("{PropertyValue} can only contain alphanumeric characters, spaces, hyphens, periods, and forward slashes.")
            .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("{PropertyValue} contains invalid characters such as '--', single quotes, or semicolons.");
    }
}

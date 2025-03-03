using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Property;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Validation.Property;

public class CreatePropertyValidator : AbstractValidator<CreatePropertyModel>
{
    public CreatePropertyValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Category id is required.")
            .NotEqual(Guid.Empty).WithMessage("Category id cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await categoryService.ExistsAsync(id)).WithMessage("Category id is not valid.")
            .DependentRules(()=> {
                RuleFor(x => x.PropName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Property name is required.")
                    .Equal(x => x.PropName.Trim()).WithMessage("Property name cannot have leading or trailing spaces.")
                    .MinimumLength(2).WithMessage("Property name must be at least 2 characters long.")
                    .MaximumLength(50).WithMessage("Property name cannot be longer than 50 characters.")
                    .Matches(@"^[a-zA-Z0-9]+(?:\s[a-zA-Z0-9]+)*$").WithMessage("Property name can only contain alphanumeric characters and spaces.")
                    .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("Property name contains invalid characters such as '--', single quotes, or semicolons.");
            });
    }
}
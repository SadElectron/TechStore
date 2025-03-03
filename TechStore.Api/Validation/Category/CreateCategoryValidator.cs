using FluentValidation;
using TechStore.Api.Models.Category;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Models.Category;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Category name is required.")
                    .Equal(x => x.CategoryName.Trim()).WithMessage("Category name cannot have leading or trailing spaces.")
                    .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.")
                    .MaximumLength(50).WithMessage("Category name cannot be longer than 50 characters.")
                    .Matches(@"^[a-zA-Z0-9]+(?:\s[a-zA-Z0-9]+)*$").WithMessage("Category name can only contain alphanumeric characters and spaces.")
                    .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("Category name contains invalid characters such as '--', single quotes, or semicolons.");
                    
    }
}

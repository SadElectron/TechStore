using FluentValidation;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Models.Category;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryModel>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .Length(3, 50).WithMessage("Category name must be between 3 and 100 characters.");
    }
}

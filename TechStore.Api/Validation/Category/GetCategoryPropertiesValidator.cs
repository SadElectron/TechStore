using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Validation.Category;

public class GetCategoryPropertiesValidator : AbstractValidator<GetCategoryPropertiesModel>
{
    public GetCategoryPropertiesValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Category id is required.")
            .NotEqual(Guid.Empty).WithMessage("Category id cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await categoryService.ExistsAsync(id)).WithMessage("Category id is not valid.");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Count must not exceed 50.");
    }
}

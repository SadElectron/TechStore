using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Validation.Category
{
    public class CategoryIdValidator : AbstractValidator<CategoryIdModel>
    {
        public CategoryIdValidator(ICategoryService categoryService)
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await categoryService.ExistsAsync(id))
            .WithMessage("Category not found.");
        }
    }
}

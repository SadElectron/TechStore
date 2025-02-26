using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Models.Category
{
    public class CategoryIdValidator : AbstractValidator<CategoryIdModel>
    {
        public CategoryIdValidator(ICategoryService categoryService)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required.")
                .NotEqual(Guid.Empty).WithMessage("ID cannot be empty.")
                .MustAsync(async (id, cancellationToken) => await categoryService.ExistsAsync(id)).WithMessage("Category id is not valid.");
        }
    }
}

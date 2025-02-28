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
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Category id is required.")
                .NotEqual(Guid.Empty).WithMessage("Category id cannot be empty.")
                .MustAsync(async (id, cancellationToken) => await categoryService.ExistsAsync(id)).WithMessage("Category id is not valid.");
        }
    }
}

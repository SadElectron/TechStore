using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Validation.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryModel>
{
    public UpdateCategoryValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Id cannot be an empty GUID.")
            .MustAsync(async (id, cancellationToken) =>
            await categoryService.ExistsAsync(id)).WithMessage("Category not found.");

        RuleFor(x => x.RowOrder)
            .GreaterThan(0).WithMessage("RowOrder must be greater than 0.")
            .MustAsync(async (rowOrder, cancellationToken) =>
            {
                var lastOrder = await categoryService.GetLastRowOrder();
                return rowOrder <= lastOrder;
            }).WithMessage("RowOrder must be lower or equal to last row order.");

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .Length(3, 50).WithMessage("Category name must be between 3 and 100 characters.");
    }
}

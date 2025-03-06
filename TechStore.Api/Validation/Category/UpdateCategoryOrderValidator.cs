using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Validation.Category;

public class UpdateCategoryOrderValidator : AbstractValidator<UpdateCategoryOrderModel>
{

    public UpdateCategoryOrderValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category id is required.")
            .NotEqual(Guid.Empty).WithMessage("Category id cannot be an empty GUID.")
            .MustAsync(async (id, cancellationToken) =>
            await categoryService.ExistsAsync(id)).WithMessage("Category not found.");

        RuleFor(x => x.RowOrder)
            .GreaterThan(0).WithMessage("RowOrder must be greater than 0.")
            .MustAsync(async (rowOrder, cancellationToken) =>
            {
                var lastOrder = await categoryService.GetLastRowOrder();
                return rowOrder <= lastOrder;
            }).WithMessage("RowOrder must be lower or equal to last row order.");
    }
}

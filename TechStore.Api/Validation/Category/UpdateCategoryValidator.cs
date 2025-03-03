using FluentValidation;
using Services.Abstract;
using TechStore.Api.Validation.Utils;


namespace TechStore.Api.Models.Category;

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
            .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Category name is required.")
                    .Equal(x => x.CategoryName.Trim()).WithMessage("Category name cannot have leading or trailing spaces.")
                    .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.")
                    .MaximumLength(50).WithMessage("Category name cannot be longer than 50 characters.")
                    .Matches(@"^[a-zA-Z0-9]+(?:[\s\-/.][a-zA-Z0-9]+)*$").WithMessage("Category name can only contain alphanumeric characters and spaces.")
                    .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("Category name contains invalid characters such as '--', single quotes, or semicolons.");
    }
}

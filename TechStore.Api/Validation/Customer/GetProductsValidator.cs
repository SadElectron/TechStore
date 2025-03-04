using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Customer;

namespace TechStore.Api.Validation.Customer;

public class GetProductsValidator : AbstractValidator<GetProductsModel>
{
    public GetProductsValidator(ICategoryService categoryService, CategoryIdValidator validator)
    {
        RuleFor(x => x.CategoryId).SetValidator(validator);
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.ItemCount)
            .GreaterThan(0).WithMessage("Item count must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Item count must not exceed 50.");
    }
}

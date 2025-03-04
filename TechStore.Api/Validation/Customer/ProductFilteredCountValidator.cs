using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Customer;

namespace TechStore.Api.Validation.Customer;

public class ProductFilteredCountValidator : AbstractValidator<ProductFilteredCountModel>
{
    public ProductFilteredCountValidator(ICategoryService categoryService)
    {
        RuleFor(x => x.CategoryId).SetValidator(new CategoryIdValidator(categoryService));
        RuleForEach(x => x.Filters).SetValidator(new ProductFilterValidator());
    }
}

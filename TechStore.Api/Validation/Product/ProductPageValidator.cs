using FluentValidation;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Validation.Product;

public class ProductPageValidator : AbstractValidator<ProductPageModel>
{
    public ProductPageValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Count must not exceed 50.");
    }
}

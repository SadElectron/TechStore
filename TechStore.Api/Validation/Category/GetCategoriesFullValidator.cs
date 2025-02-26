using FluentValidation;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Models.Category;

public class GetCategoriesFullValidator : AbstractValidator<GetCategoriesFullModel>
{
    public GetCategoriesFullValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Count must not exceed 100.");

        RuleFor(x => x.ProductPage)
            .GreaterThan(0).WithMessage("Product page number must be greater than 0.");

        RuleFor(x => x.ProductCount)
            .GreaterThan(0).WithMessage("Product count must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Count must not exceed 100.");
    }
}

using FluentValidation;

namespace TechStore.Api.Models.Product;

public class GetProductsValidator : AbstractValidator<(int page, int count)>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.count)
            .GreaterThan(0).WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Count must not exceed 100.");
    }
}

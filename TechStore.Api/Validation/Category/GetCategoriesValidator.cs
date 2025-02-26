using FluentValidation;
using TechStore.Api.Models.Category;

namespace TechStore.Api.Models.Category;

public class GetCategoriesValidator : AbstractValidator<GetCategoriesModel>
{
    public GetCategoriesValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Count must not exceed 100.");
    }
}

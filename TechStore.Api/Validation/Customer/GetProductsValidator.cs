using FluentValidation;
using Services.Abstract;
using System.Threading;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Customer;

namespace TechStore.Api.Validation.Customer;

public class GetProductsValidator : AbstractValidator<GetProductsModel>
{
    public GetProductsValidator(ICategoryService categoryService, IValidator<CategoryIdModel> validator)
    {
        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .CustomAsync(async (id, context, cancellationToken) =>
            {
                var result = await validator.ValidateAsync(new CategoryIdModel { Id = id }, cancellationToken);
                if (!result.IsValid)
                {
                    context.AddFailure(result.Errors.First());
                }
            });
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.ItemCount)
            .GreaterThan(0).WithMessage("Item count must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Item count must not exceed 50.");
    }
}

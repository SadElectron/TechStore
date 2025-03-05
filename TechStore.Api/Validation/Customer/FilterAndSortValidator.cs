using Core.RequestModels;
using FluentValidation;

namespace TechStore.Api.Validation.Customer;

public class FilterAndSortValidator : AbstractValidator<FilterAndSortModel>
{
    private readonly string[] _allowedSortColumns = { "price", "date"};
    private readonly string[] _allowedSortValues = { "asc", "desc" };
    public FilterAndSortValidator()
    {
        RuleFor(x => x.sort)
            .Must(x => _allowedSortColumns.Contains(x.ToLower())).WithMessage("Invalid sort column.");

        RuleFor(x => x.sortValue)
            .Must(x => _allowedSortValues.Contains(x.ToLower())).WithMessage("Invalid sort value.");

        RuleForEach(x => x.filters).SetValidator(new ProductFilterValidator());
    }
}

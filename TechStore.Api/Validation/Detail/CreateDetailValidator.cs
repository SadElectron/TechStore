using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Detail;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Models.Detail;

public class CreateDetailValidator : AbstractValidator<CreateDetailModel>
{
    public CreateDetailValidator(IProductService productService, IPropertyService propertyService)
    {
        RuleFor(x => x.PropValue)
            .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("PropValue is required.")
                    .Equal(x => x.PropValue.Trim()).WithMessage("PropValue cannot have leading or trailing spaces.")
                    .MinimumLength(3).WithMessage("PropValue must be at least 3 characters long.")
                    .MaximumLength(50).WithMessage("PropValue cannot be longer than 50 characters.")
                    .Matches(@"^[a-zA-Z0-9\-.\/]+(?:\s[a-zA-Z0-9\-.\/]+)*$").WithMessage("PropValue can only contain alphanumeric characters, spaces, hyphens, periods, and forward slashes.")
                    .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("PropValue contains invalid characters such as '--', single quotes, or semicolons.");

        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required.")
            .MustAsync(async (id, cancellationToken) => await propertyService.ExistsAsync(id))
            .WithMessage("Invalid PropertyId.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .MustAsync(async (id, cancellationToken) => await productService.ExistsAsync(id))
            .WithMessage("Invalid ProductId.");
    }
}

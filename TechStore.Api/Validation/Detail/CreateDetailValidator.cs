using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Detail;

namespace TechStore.Api.Models.Detail;

public class CreateDetailValidator : AbstractValidator<CreateDetailModel>
{
    public CreateDetailValidator(IProductService productService, IPropertyService propertyService)
    {
        RuleFor(x => x.PropValue)
            .NotEmpty().WithMessage("Property value is required.")
            .Length(2, 50).WithMessage("Property value cannot exceed 255 characters.");

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

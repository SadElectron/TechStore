using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Models.Product;

public class ProductIdValidator : AbstractValidator<ProductIdModel>
{
    public ProductIdValidator( IProductService productService)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.")
            .NotEqual(Guid.Empty).WithMessage("ID cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await productService.ExistsAsync(id)).WithMessage("Product id is not valid.");
    }
}

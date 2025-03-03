using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Models.Product;

public class DeleteProductValidator : AbstractValidator<DeleteProductModel>
{
    public DeleteProductValidator(IProductService productService)
    {
        
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product id is required and cannot be empty.")
            .NotEqual(Guid.Empty).WithMessage("Product id cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await productService.ExistsAsync(id)).WithMessage("Product not found.");
    }
}

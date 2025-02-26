using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Models.Product;

public class DeleteProductValidator : AbstractValidator<DeleteProductModel>
{
    private readonly IProductService _productService;
    public DeleteProductValidator(IProductService productService)
    {
        _productService = productService;
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await _productService.ExistsAsync(id))
            .WithMessage("Product not found.");
    }
}

using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models;

namespace TechStore.Api.Validation;

public class UpdateProductModelValidator : AbstractValidator<UpdateProductModel>
{
    private readonly IProductService _productService;
    public UpdateProductModelValidator(IProductService productService)
    {
        _productService = productService;

        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await _productService.ExistsAsync(id))
            .WithMessage("Product not found.");

        RuleFor(p => p.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(2).WithMessage("Product name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

        RuleFor(p => p.SoldQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Sold quantity cannot be negative.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}

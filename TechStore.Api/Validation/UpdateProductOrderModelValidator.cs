using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models;

namespace TechStore.Api.Validation;

public class UpdateProductOrderModelValidator : AbstractValidator<UpdateProductOrderModel>
{
    private readonly IProductService _productService;
    public UpdateProductOrderModelValidator(IProductService productService)
    {
       
        _productService = productService;
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await _productService.ExistsAsync(id))
            .WithMessage("Product not found.");
        RuleFor(p => p.ProductOrder)
            .GreaterThan(0).WithMessage("Product order must be greater than zero.")
            .MustAsync(async (productOrder, cancellationToken) => 
                productOrder <= await _productService.GetLastProductOrderAsync()).WithMessage("Product order must be lower or equal to last product order.");
    }
}

using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Product;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Models.Product;

public class UpdateProductModelValidator : AbstractValidator<UpdateProductModel>
{
    public UpdateProductModelValidator(IProductService productService)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await productService.ExistsAsync(id))
            .WithMessage("Product not found.");

        RuleFor(p => p.ProductName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Product name is required.")
            .Equal(x => x.ProductName.Trim()).WithMessage("Product name cannot have leading or trailing spaces.")
            .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Product name cannot be longer than 50 characters.")
            .Matches(@"^[a-zA-Z0-9]+(?:\s[a-zA-Z0-9]+)*$").WithMessage("Product name can only contain alphanumeric characters and spaces.")
            .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("Product name contains invalid characters such as '--', single quotes, or semicolons.");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

        RuleFor(p => p.SoldQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Sold quantity cannot be negative.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}

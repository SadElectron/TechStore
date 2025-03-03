namespace TechStore.Api.Models.Product;

using FluentValidation;
using Microsoft.Win32;
using Services.Abstract;
using TechStore.Api.Models.Product;
using TechStore.Api.Validation.Utils;

public class CreateProductModelValidator : AbstractValidator<CreateProductModel>
{
    private readonly ICategoryService _categoryService;

    public CreateProductModelValidator(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required and cannot be empty.")
            .MustAsync(async (categoryId, cancellationToken) =>
                await _categoryService.ExistsAsync(categoryId))
            .WithMessage("Category not found.");

        RuleFor(x => x.ProductName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Product name is required.")
            .Equal(x => x.ProductName.Trim()).WithMessage("Product name cannot have leading or trailing spaces.")
            .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Product name cannot be longer than 50 characters.")
            .Matches(@"^[a-zA-Z0-9]+(?:[\s\-/.][a-zA-Z0-9]+)*$").WithMessage("Product name can only contain alphanumeric characters and spaces.")
            .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x)).WithMessage("Product name contains invalid characters such as '--', single quotes, or semicolons.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(1).WithMessage("Stock must be at least 1.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}


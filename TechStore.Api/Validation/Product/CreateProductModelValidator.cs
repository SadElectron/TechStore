namespace TechStore.Api.Models.Product;

using FluentValidation;
using Microsoft.Win32;
using Services.Abstract;
using TechStore.Api.Models.Product;

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
            .NotEmpty().WithMessage("ProductName is required.")
            .MinimumLength(2).WithMessage("ProductName must be at least 2 characters long.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(1).WithMessage("Stock must be at least 1.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}


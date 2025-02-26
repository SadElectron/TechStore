using FluentValidation;
using Services.Abstract;

namespace TechStore.Api.Models.Product;

public class GetProductCountValidator: AbstractValidator<Guid>
{
    private readonly ICategoryService _categoryService;
    public GetProductCountValidator(ICategoryService categoryService)
    {
        _categoryService = categoryService;
        RuleFor(p => p)
            .NotEmpty().WithMessage("Category id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await _categoryService.ExistsAsync(id))
            .WithMessage("Category not found.");
    }
}

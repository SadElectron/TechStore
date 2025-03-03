using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Product;

namespace TechStore.Api.Models.Product;

public class UpdateProductOrderModelValidator : AbstractValidator<UpdateProductOrderModel>
{

    public UpdateProductOrderModelValidator(IProductService productService)
    {

        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Product id is required and cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await productService.ExistsAsync(id)).WithMessage("Product not found.")
            .DependentRules(() =>
                {
                    RuleFor(p => p.ProductOrder)
                        .Cascade(CascadeMode.Stop)
                        .GreaterThan(0).WithMessage("Product order must be greater than zero.")
                        .MustAsync(async (model, productOrder, cancellationToken) => productOrder <= await productService.GetLastProductOrderByProductIdAsync(model.Id))
                        .WithMessage("Product order must be lower or equal to last product order.");
                });
    }
}

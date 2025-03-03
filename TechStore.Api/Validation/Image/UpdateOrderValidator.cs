using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Image;

namespace TechStore.Api.Validation.Image;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderModel>
{
    public UpdateOrderValidator(IImageService imageService)
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Image ID is required.")
            .MustAsync(async (id, cancellationToken) => await imageService.ExistsAsync(id)).WithMessage("Image ID is not valid.")
            .DependentRules(() =>
            {
                RuleFor(x => x.NewOrder)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("New order must be greater than 0.")
                .MustAsync(async (model, newOrder, cancellationToken) => newOrder <= await imageService.GetLastImageOrderAsync(model.Id)).WithMessage("New order must be less than or equal to the last image order.");
            });
    }
}

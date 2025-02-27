using FluentValidation;
using Services.Abstract;
using Services.Concrete;
using TechStore.Api.Models.Image;

namespace TechStore.Api.Models.Image;

public class ImageIdValidator : AbstractValidator<ImageIdModel>
{
    public ImageIdValidator(IImageService imageService)
    {
        RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Image id is required.")
                .NotEqual(Guid.Empty).WithMessage("Image id cannot be empty.")
                .MustAsync(async (id, cancellationToken) => await imageService.ExistsAsync(id)).WithMessage("Image id is not valid.");
    }
}

using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Image;

namespace TechStore.Api.Models.Image;

public class UploadImagesValidator : AbstractValidator<UploadImagesModel>
{
    private static readonly string[] AllowedFileTypes = { ".jpg", ".jpeg", ".png", ".webp" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
    public UploadImagesValidator(IProductService productService, ILogger<UploadImagesValidator> logger)
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Product ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Product ID cannot be empty.")
            .MustAsync(async (id, cancellationToken) => await productService.ExistsAsync(id)).WithMessage("Product ID is not valid.");

        RuleFor(x => x.Files)
            .Cascade(CascadeMode.Stop)
            .Must(files => files != null && files.Any()).WithMessage("No files provided.")
            .Must(files => files.Count <= 5).WithMessage("A maximum of 5 files can be uploaded at once.")
            .DependentRules(() =>
            {
                 RuleForEach(x => x.Files)
                    .Cascade(CascadeMode.Stop)
                    .Must(file => file != null && file.Length > 0).WithMessage("Each file must be provided and non-empty.")
                    .Must(file => file.Length <= MaxFileSize).WithMessage($"Each file must not exceed {MaxFileSize / (1024 * 1024)} MB.")
                    .Must(file => file.ContentType.StartsWith("image/")).WithMessage("Each file must be an image.")
                    .Must(file => AllowedFileTypes.Contains(Path.GetExtension(file.FileName).ToLower())).WithMessage("Each file must be a valid image format (.jpg, .jpeg, .png, .webp).");
            });



    }
}

using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Detail;

namespace TechStore.Api.Models.Detail;

public class UpdateDetailValidator : AbstractValidator<UpdateDetailModel>
{
    public UpdateDetailValidator(IDetailService detailService)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.")
            .NotEqual(Guid.Empty).WithMessage("ID cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await detailService.ExistsAsync(id))
            .WithMessage("Detail id is not valid.");
        
        RuleFor(x => x.PropValue)
            .NotEmpty().WithMessage("PropValue is required.")
            .MinimumLength(2).WithMessage("PropValue must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("PropValue must not exceed 100 characters.");
    }
}

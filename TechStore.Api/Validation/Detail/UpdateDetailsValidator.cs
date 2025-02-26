using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Detail;

namespace TechStore.Api.Validation.Detail;

public class UpdateDetailsValidator : AbstractValidator<List<UpdateDetailModel>>
{
    public UpdateDetailsValidator(IDetailService detailService)
    {
        RuleForEach(x => x)
            .ChildRules(detail =>
            {
                detail.RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("ID is required.")
                    .NotEqual(Guid.Empty).WithMessage("ID cannot be empty.")
                    .MustAsync(async (id, cancellationToken) =>
                        await detailService.ExistsAsync(id))
                    .WithMessage(x => $"Detail id {x.Id} is not valid.");

                detail.RuleFor(x => x.PropValue)
                    .NotEmpty().WithMessage("PropValue is required.")
                    .MinimumLength(3).WithMessage(x => $"PropValue {x.PropValue} must be at least 3 characters long.")
                    .MaximumLength(100).WithMessage(x => $"PropValue {x.PropValue} must not exceed 100 characters.");
            });
    }
}

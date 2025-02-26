using FluentValidation;
using Services.Abstract;
using TechStore.Api.Models.Detail;

namespace TechStore.Api.Models.Detail;

public class DetailIdValidator : AbstractValidator<DetailIdModel>
{
    public DetailIdValidator(IDetailService detailService)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.")
            .NotEqual(Guid.Empty).WithMessage("ID cannot be empty.")
            .MustAsync(async (id, cancellationToken) =>
                await detailService.ExistsAsync(id))
            .WithMessage("Detail id is not valid.");
    }
}

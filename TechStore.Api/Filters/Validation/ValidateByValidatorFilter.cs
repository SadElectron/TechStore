using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechStore.Api.Filters.Validation;

public class ValidateByValidatorFilter<TValidator, TModel> : IAsyncActionFilter where TValidator : AbstractValidator<TModel>
{
    private readonly TValidator _validator;

    public ValidateByValidatorFilter(TValidator validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        /*if (context.ActionArguments.TryGetValue("model", out var argument) && argument is TValidator model)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(new { message = "Validation failed.", errors = errorMessages });
                return;
            }
        }*/

        await next();
    }
}

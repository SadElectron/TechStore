using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechStore.Api.Filters.Validation;

public class ValidateByModelFilter<T> : IAsyncActionFilter where T : class
{
    private readonly IValidator<T> _validator;
    private readonly ILogger<ValidateByModelFilter<T>> _logger;

    public ValidateByModelFilter(IValidator<T> validator, ILogger<ValidateByModelFilter<T>> logger)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            if (context.ActionArguments.TryGetValue("model", out var argument) && argument is T model)
            {
                ValidationResult validationResult = await _validator.ValidateAsync(model);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    context.Result = new BadRequestObjectResult(new { message = "Validation failed.", errors = errorMessages });
                    return;
                }
            }

            await next();
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in ValidateByModelFilter.OnActionExecutionAsync {ex.Message}");
        }
        
    }
}

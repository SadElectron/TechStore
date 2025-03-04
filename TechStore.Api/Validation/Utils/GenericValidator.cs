using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models.Validation;

namespace TechStore.Api.Validation.Utils;

public class GenericValidator<TValidationModel, TController>
{
    private readonly ILogger<TController> _logger;
    private readonly IValidator<TValidationModel> _validator;

    public GenericValidator(ILogger<TController> logger, IValidator<TValidationModel> validator)
    {
        _logger = logger;
        _validator = validator;
    }
    public async Task<ValidationResultModel> ValidateAsync(TValidationModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return new ValidationResultModel
            {
                IsValid = false,
                Response = new("Validation failed.", errorMessages)
            };
        }
        return new ValidationResultModel { IsValid = true };
    }
}

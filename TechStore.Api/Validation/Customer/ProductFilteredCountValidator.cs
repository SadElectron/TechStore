﻿using FluentValidation;
using Services.Abstract;
using System.ComponentModel.DataAnnotations;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Customer;

namespace TechStore.Api.Validation.Customer;

public class ProductFilteredCountValidator : AbstractValidator<ProductFilteredCountModel>
{
    public ProductFilteredCountValidator(ICategoryService categoryService, IValidator<CategoryIdModel> validator)
    {
        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .CustomAsync(async (id, context, cancellationToken) =>
            {
                var result = await validator.ValidateAsync(new CategoryIdModel { Id = id }, cancellationToken);
                if (!result.IsValid)
                {
                    context.AddFailure(result.Errors.First());
                }
            });
        RuleForEach(x => x.Filters).SetValidator(new ProductFilterValidator());
    }
}

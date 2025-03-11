using Core.Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TechStore.Api.Models.Auth;

namespace TechStore.Api.Validation.Auth;

public class RegisterValidator : AbstractValidator<RegisterModel>
{
    public RegisterValidator(UserManager<CustomIdentityUser> userManager)
    {

        // Email validation
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.")
            .MustAsync(async (email, cancellation) => {
                var userCheck = await userManager.FindByEmailAsync(email);
                return userCheck == null;
            }).WithMessage("User with this email already exists.");

        // Password validation
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");

        // Confirm password validation
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(x => x.Password).WithMessage("Password and Confirmation Password must match.");
    }
}

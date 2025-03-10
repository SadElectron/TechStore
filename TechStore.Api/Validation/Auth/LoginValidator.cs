using Core.Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TechStore.Api.Models.Auth;
using TechStore.Api.Validation.Utils;

namespace TechStore.Api.Validation.Auth;

public class LoginValidator: AbstractValidator<LoginModel>
{
    public LoginValidator(UserManager<CustomIdentityUser> userManager)
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .Equal(x => x.Email.Trim()).WithMessage("Email cannot have leading or trailing spaces.")
            .Matches(@"^[a-zA-Z0-9\-._@+]+$")
            .WithMessage("Email contains invalid characters. Only the following characters are allowed: a-z, A-Z, 0-9, '-', '.', '_', '@', '+'.")
            .Must(x => !ValidationUtils.ContainsSuspiciousCharacters(x))
            .WithMessage("Email contains invalid characters such as '--', single quotes, or semicolons.")
            .MustAsync(async (email, cancellation) =>
            {
                var user = await userManager.FindByEmailAsync(email);
                return user != null;
            }).WithMessage("Invalid email or password");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}

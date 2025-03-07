using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Abstract;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Concrete;

public partial class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    public AuthService(ITokenService tokenService, UserManager<CustomIdentityUser> userManager, SignInManager<CustomIdentityUser> signInManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    public async Task<RegisterUserResult> Register(CustomIdentityUser user, string password)
    {
        var timeNow = DateTimeHelper.GetUtcNow();
        user.Created = timeNow;
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.Create(user, userRoles);

            // Generate refresh token
            var refreshToken = _tokenService.CreateRefreshToken();

            // Store refresh token (hashed) in the database
            user.RefreshToken = _tokenService.HashToken(refreshToken);
            user.RefreshTokenExpiryTime = timeNow.AddHours(Convert.ToDouble(_configuration["TokenSettings:RefreshTokenExpirationInMinutes"]));
            await _userManager.UpdateAsync(user);

            return new RegisterUserResult
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresIn = timeNow.AddHours(2),
                IsSuccessful = true
            };
        }

        return new RegisterUserResult
        {
            IsSuccessful = false,
            Errors = result.Errors.Select(e => e.Description).ToList()
        };

    }
    public async Task<LoginResult> Login(string email, string passwd)
    {
        var timeNow = DateTimeHelper.GetUtcNow();
        var result = await _signInManager.PasswordSignInAsync(email, email, false, false);
        if (result.Succeeded)
        {
            //var refreshToken = await _authService.AddRefreshTokenAsync(result.Id);
            //return Ok(new { message = "Logged in successfully", result.Token, refreshToken });
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user!);
            var token = _tokenService.Create(user!, roles);
            var refreshToken = _tokenService.CreateRefreshToken();

            // Store refresh token (hashed) in the database
            user!.RefreshToken = _tokenService.HashToken(refreshToken);
            user.RefreshTokenExpiryTime = timeNow.AddMinutes(Convert.ToDouble(_configuration["TokenSettings:RefreshTokenExpirationInMinutes"]));
            await _userManager.UpdateAsync(user);
            return new LoginResult { Token = token, RefreshToken = refreshToken, ExpiresIn = user.RefreshTokenExpiryTime, IsSuccessful = true };
        }
        return new LoginResult { Errors = new() { "Invalid email or password" }, IsSuccessful = false };
    }

    public async Task<LoginResult> Refresh(string token, string refreshToken)
    {
        var timeNow = DateTimeHelper.GetUtcNow();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == _tokenService.HashToken(refreshToken));
        if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return new LoginResult { Errors = ["Invalid or expired refresh token"], IsSuccessful = false };
        }
        // Get user roles
        var userRoles = await _userManager.GetRolesAsync(user);

        // Generate new JWT token
        var newJwtToken = _tokenService.Create(user, userRoles);

        // Generate new refresh token
        var newRefreshToken = _tokenService.CreateRefreshToken();
        user.RefreshToken = _tokenService.HashToken(newRefreshToken);
        user.RefreshTokenExpiryTime = timeNow.AddHours(2);
        await _userManager.UpdateAsync(user);
        return new LoginResult { Token = newJwtToken, RefreshToken = newRefreshToken, ExpiresIn = user.RefreshTokenExpiryTime, IsSuccessful = true };
    }
    public async Task<LogoutResult> Logout(string userId)
    {
        // Find the user
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new LogoutResult { Message = "User not found", IsSuccessful = false };
        }

        // Invalidate the refresh token
        user.RefreshToken = "INVALIDATED_" + Guid.NewGuid().ToString();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-1); // Set to past time
        await _userManager.UpdateAsync(user);

        // Log the logout event (optional)
        // await _logger.LogInformationAsync($"User {user.Email} logged out at {DateTime.UtcNow}");

        return new LogoutResult { Message = "Logged out successfully", IsSuccessful = true };
    }
}

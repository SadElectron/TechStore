using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Abstract;

namespace Services.Concrete;

public partial class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly RoleManager<CustomIdentityRole> _roleManager;

    public AuthService(
        ITokenService tokenService,
        UserManager<CustomIdentityUser> userManager,
        IConfiguration configuration,
        RoleManager<CustomIdentityRole> roleManager,
        ITokenBlacklistService tokenBlacklistService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _tokenBlacklistService = tokenBlacklistService;
    }
    public async Task<RegisterUserResult> Register(CustomIdentityUser user, string password)
    {
        await CheckRoles();
        user.Created = DateTimeHelper.GetUtcNow();
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.Create(user, userRoles);
            var refreshToken = _tokenService.CreateRefreshToken(user);

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new RegisterUserResult
            {
                Token = token,
                RefreshToken = refreshToken,
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
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.CheckPasswordAsync(user!, passwd);
        if (result)
        {
            var roles = await _userManager.GetRolesAsync(user!);
            var token = _tokenService.Create(user!, roles);
            var refreshToken = _tokenService.CreateRefreshToken(user!);

            user!.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);
            return new LoginResult { Token = token, RefreshToken = refreshToken, IsSuccessful = true };
        }
        return new LoginResult { Errors = new() { "Invalid email or password" }, IsSuccessful = false };
    }
    public async Task<LoginResult> Refresh(string refreshToken)
    {
        var timeNow = DateTimeHelper.GetUtcNow();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null)
        {
            return new LoginResult { Errors = ["Invalid or expired refresh token"], IsSuccessful = false };
        }
        var userRoles = await _userManager.GetRolesAsync(user);
        var newJwtToken = _tokenService.Create(user, userRoles);
        var newRefreshToken = _tokenService.CreateRefreshToken(user);

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        return new LoginResult { Token = newJwtToken, RefreshToken = newRefreshToken, IsSuccessful = true };
    }
    public async Task<LogoutResult> Logout(string userId, string jti)
    {
        var timeNow = DateTimeHelper.GetUtcNow();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new LogoutResult { Message = "User not found", IsSuccessful = false };
        }

        user.RefreshToken = "INVALIDATED_" + Guid.NewGuid().ToString();
        await _userManager.UpdateAsync(user);

        if (!await _tokenBlacklistService.IsTokenBlacklistedAsync(jti)) await _tokenBlacklistService.AddTokenToBlacklistAsync(jti, timeNow);

        // await _logger.LogInformationAsync($"User {user.Email} logged out at {DateTime.UtcNow}");

        return new LogoutResult { Message = "Logged out successfully", IsSuccessful = true };
    }
    private async Task CheckRoles()
    {
        foreach (var role in new[] { "Admin", "Customer", "Manager" })
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new CustomIdentityRole { Name = role });
            }
        }
    }
}

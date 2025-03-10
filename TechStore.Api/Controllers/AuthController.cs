using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechStore.Api.Dtos;
using TechStore.Api.Filters.Validation;
using TechStore.Api.Models.Auth;
using TechStore.Api.Validation.Auth;

namespace TechStore.Api.Models;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("login")]
    [TypeFilter(typeof(ValidateByModelFilter<LoginModel>))]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            LoginResult result = await _authService.Login(model.Email, model.Password);
            if (result.IsSuccessful)
            {
                return Ok(new { result.Token, result.RefreshToken, result.ExpiresIn });
            }
            return BadRequest(new { message = "Login failed", errors = result.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in AuthController.Login {ex.Message}");
            return Problem();
        }
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Get the current user ID from the JWT token claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(new { Message = "Unable to identify user" });
        }
        LogoutResult result =  await _authService.Logout(userId);
        if (result.IsSuccessful)
        {
            return Ok(new { message = "Logged out successfully" });
        }
        return BadRequest(new { message = "Logout failed" });

    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshModel model)
    {
        _logger.LogInformation($"User {User}");
        try
        {
            LoginResult result = await _authService.Refresh(model.Token, model.RefreshToken);
            if (result.IsSuccessful)
            {
                return Ok(new { result.Token, result.RefreshToken, result.ExpiresIn });
            }
            return BadRequest(new { message = "Token refresh failed", errors = result.Errors });
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in AuthController.Refresh {ex.Message}");
            return Problem();
        }
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            var user = new CustomIdentityUser { UserName = model.Email, Email = model.Email, };
            RegisterUserResult result = await _authService.Register(user, model.Password);
            if (result.IsSuccessful)
            {
                return Ok(new { result.Token, result.RefreshToken, result.ExpiresIn });
            }
            return BadRequest(new { message = "User registration failed", errors = result.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in AuthController.Register {ex.Message}");
            return Problem();
        }
    }
}

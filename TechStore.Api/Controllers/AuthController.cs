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
    private readonly IAuthService _authService;
    private readonly ITokenBlacklistService _tokenBlacklistService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ILogger<AuthController> logger,
        IAuthService authService,
        ITokenBlacklistService tokenBlacklistService)
    {
        _logger = logger;
        _authService = authService;
        _tokenBlacklistService = tokenBlacklistService;
    }

    [HttpPost("Register")]
    [TypeFilter(typeof(ValidateByModelFilter<RegisterModel>))]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            var user = new CustomIdentityUser { UserName = model.Email, Email = model.Email, };
            RegisterUserResult result = await _authService.Register(user, model.Password);
            if (result.IsSuccessful)
            {
                return Ok(new { result.Token, result.RefreshToken });
            }
            return BadRequest(new { message = "User registration failed", errors = result.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in AuthController.Register {ex.Message}");
            return Problem();
        }
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
                return Ok(new { result.Token, result.RefreshToken});
            }
            return BadRequest(new { message = "Login failed", errors = result.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error in AuthController.Login {ex.Message}");
            return Problem();
        }
    }

    [Authorize]
    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            LoginResult result = await _authService.Refresh(token);
            if (result.IsSuccessful)
            {
                return Ok(new { result.Token, result.RefreshToken});
            }
            return BadRequest(new { message = "Token refresh failed", errors = result.Errors });
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in AuthController.Refresh {ex.Message}");
            return Problem();
        }
    }

    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (string.IsNullOrEmpty(jti))
                return BadRequest(new { message = "Invalid token format" });


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "Unable to identify user" });
            }

            LogoutResult result = await _authService.Logout(userId, jti);

            if (result.IsSuccessful)
            {
                return Ok(new { message = "Logged out successfully" });
            }
            return BadRequest(new { message = "Logout failed" });
        }
        catch (Exception ex)
        {

            _logger.LogWarning($"Error in AuthController.Logout {ex.Message}");
            return Problem();
        }
        

    }


}

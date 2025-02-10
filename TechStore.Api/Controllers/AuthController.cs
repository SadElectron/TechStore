using Core.Dtos;
using Core.Entities.Concrete;
using DataAccess.Migrations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Authentication.Jwt;

namespace TechStore.Api.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public record UserReq(string email, string password);
    public record RefreshTokenReq(string RefreshToken);
    public record LogoutReq(string RefreshToken);


    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserReq userRequest)
    {
        var result = await _authService.Login(userRequest.email, userRequest.password);
        if (result.Status)
        {
            var refreshToken = await _authService.AddRefreshTokenAsync(result.Id);
            return Ok(new { message = "Logged in successfully", result.Token, refreshToken });
           
        }
        return NotFound();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody]LogoutReq logoutReq)
    {
        await _authService.DeleteTokenAsync(logoutReq.RefreshToken);
        return Ok(new { message = "Logged out successfully"});
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody]RefreshTokenReq refreshTokenReq)
    {
        var validationResult = await _authService.ValidateToken(refreshTokenReq.RefreshToken);
        if (validationResult)
        {
            var token = await _authService.CreateTokenAsync(refreshTokenReq.RefreshToken);
            return Ok(new { token });
        }
        else
        {
            return NotFound();
        }
        
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserDto userReq)
    {
        var user = new User
        {
            Email = userReq.Email,
            UserName = userReq.UserName,
            Password = userReq.Password,
            LastUpdate = DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond),
            CreatedAt = DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond)
        };
        RegisterUserResult registerUserResult = await _authService.Register(user, "Customer");
        return registerUserResult.success ?
             Ok() : BadRequest(new {Error = registerUserResult.failReason});
    }

}

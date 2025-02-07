using Core.Dtos;
using Core.Entities.Concrete;
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

    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    private readonly IRefreshTokenService _refreshTokenService;
    public AuthController(ILogger<AuthController> logger, IAuthService authService, IRefreshTokenService refreshTokenService)
    {
        _logger = logger;
        _authService = authService;
        _refreshTokenService = refreshTokenService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserReq userRequest)
    {
        var result = await _authService.Login(userRequest.email, userRequest.password);
        if (!result.Status)
        {
            return NotFound();
        }

        await _refreshTokenService.AddTokenAsync(result.Id);
        return Ok(new { message = "Logged in successfully", result.Id, email = result.Email, token = result.Token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody]string refreshToken)
    {
        await _refreshTokenService.DeleteTokenAsync(refreshToken);
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody]string refreshToken, [FromBody]Guid userId)
    {
        var result = await _authService.Refresh(refreshToken, userId);

        if (result.Status)
        {
            return Ok(new { token = result.Token });
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
            Password = userReq.Password
        };
        RegisterUserResult registerUserResult = await _authService.Register(user, "Customer");
        return registerUserResult.success ?
             Ok() : BadRequest();
    }

}

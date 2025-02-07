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
    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserReq userRequest)
    {
        var result = await _authService.Login(userRequest.email, userRequest.password);
        if (!result.Status)
        {
            return NotFound();
        }

        await _authService.AddRefreshTokenAsync(result.Id);
        return Ok(new { message = "Logged in successfully", result.Id, email = result.Email, token = result.Token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody]string refreshToken)
    {
        await _authService.DeleteTokenAsync(refreshToken);
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody]string refreshToken, [FromBody]Guid userId)
    {
        var validationResult = await _authService.ValidateToken(refreshToken, userId);
        if (validationResult)
        {
            var token = await _authService.CreateTokenAsync(userId);
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
            Password = userReq.Password
        };
        RegisterUserResult registerUserResult = await _authService.Register(user, "Customer");
        return registerUserResult.success ?
             Ok() : BadRequest();
    }

}

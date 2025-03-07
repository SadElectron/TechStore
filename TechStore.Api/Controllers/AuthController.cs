using Core.Entities.Concrete;
using Core.Results;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using TechStore.Api.Dtos;
using TechStore.Api.Models.Auth;

namespace TechStore.Api.Models;

[EnableCors("AllowSpecificOrigin")]
[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public record UserReq(string email, string password);
    public record RefreshTokenReq(string RefreshToken);
    public record LogoutReq(string RefreshToken);


    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ILogger<AuthController> logger,
        ITokenService tokenService,
        UserManager<CustomIdentityUser> userManager,
        SignInManager<CustomIdentityUser> signInManager)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            //var refreshToken = await _authService.AddRefreshTokenAsync(result.Id);
            //return Ok(new { message = "Logged in successfully", result.Token, refreshToken });
            var user = await _userManager.FindByNameAsync(model.Email);
            var token = _tokenService.Create(user);
            return Ok(new { Token = token });
        }
        return NotFound();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutReq logoutReq)
    {
        await _authService.DeleteTokenAsync(logoutReq.RefreshToken);
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenReq refreshTokenReq)
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
            Password = userReq.Password
        };
        RegisterUserResult registerUserResult = await _authService.Register(user, "Customer");
        return registerUserResult.success ?
             Ok() : BadRequest(new { Error = registerUserResult.failReason });
    }

}

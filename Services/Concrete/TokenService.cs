using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenDal _refreshTokenDal;
    private readonly UserManager<CustomIdentityUser> _userManager;
    public TokenService(IConfiguration configuration, UserManager<CustomIdentityUser> userManager, IRefreshTokenDal refreshTokenDal)
    {
        _configuration = configuration;
        _userManager = userManager;
        _refreshTokenDal = refreshTokenDal;
    }
    public string Create(CustomIdentityUser user)
    {
        var secretKey = _configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Verified", user.Verified.ToString()),
                    new Claim("Role", "Customer"),
                ]),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }
    public async Task<string> RefreshAsync(string refreshToken)
    {
        var rt = await _refreshTokenDal.GetWithUserAsync(refreshToken);

        var newToken = Create(rt!.User);

        return token;
    }
}

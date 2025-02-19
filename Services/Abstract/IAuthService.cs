using Core.Entities.Concrete;
using Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract;

public interface IAuthService
{
    Task<RegisterUserResult> Register(User user, string role);
    Task<LoginResult> Login(string email, string passwd);
    Task<bool> ValidateToken(string token);
    Task<string> AddRefreshTokenAsync(Guid userId);
    Task DeleteTokenAsync(string token);
    Task<string> CreateTokenAsync(string refreshToken);
}

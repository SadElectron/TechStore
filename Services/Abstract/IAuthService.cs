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
    Task<RegisterUserResult> Register(CustomIdentityUser user, string role);
    Task<LoginResult> Login(string email, string passwd);
    Task<LoginResult> Refresh(string refreshToken);
    Task<LogoutResult> Logout(string userId, string jti);
}

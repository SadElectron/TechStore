using Core.Dtos;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Services.Abstract;
using Services.Authentication.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Concrete
{
    public partial class AuthService : IAuthService
    {
        private readonly IAuthDal _authDal;
        private readonly IRefreshTokenDal _refreshTokenDal;
        private readonly TokenProvider _tokenProvider;
        public AuthService(IAuthDal authDal, TokenProvider tokenProvider, IRefreshTokenDal refreshTokenDal)
        {
            _authDal = authDal;
            _tokenProvider = tokenProvider;
            _refreshTokenDal = refreshTokenDal;
        }

        public async Task<LoginResult> Login(string email, string passwd)
        {
            var passwordHash = SHA256.HashData(Encoding.UTF8.GetBytes(passwd));
            var passwordHashString = Convert.ToHexStringLower(passwordHash);

            var userDb = await _authDal.GetAsNoTrackingAsync(u => u.Email == email && u.Password == passwordHashString);
            if (userDb == null || userDb.Order == 0)
            {
                return new LoginResult(default, "", "", false);
            }
            if (userDb.Email == email && userDb.Password == passwordHashString)
            {
                return new LoginResult(userDb.Id, userDb.Email, _tokenProvider.Create(userDb), true);

            }

            return new LoginResult(default, "", "", false);
        }
        public

        public async Task<bool> ValidateToken(string token, Guid userId)
        {
            var refreshToken = await _refreshTokenDal.GetAsNoTrackingAsync(r => r.Token == token);
            if (refreshToken == null) return false;
            return refreshToken.UserId == userId;
        }

        public async Task<string> AddTokenAsync(Guid userId)
        {
            byte[] randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            string refreshToken = Convert.ToBase64String(randomBytes);
            var addResult = await _refreshTokenDal.AddAsync(new() { Id = Guid.NewGuid(), Token = refreshToken, UserId = userId });
            return refreshToken;
        }

        public async Task DeleteTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenDal.GetAsNoTrackingAsync(r => r.Token == token);
            await _refreshTokenDal.DeleteAsync(refreshToken!);
        }

        public async Task<RegisterUserResult> Register(User user, string role)
        {
            bool emailCheck = EmailRegex().IsMatch(user.Email);
            bool userNameCheck = UserNameRegex().IsMatch(user.UserName);
            bool passwordCheck = PasswordRegex().IsMatch(user.Password);
            var passwd8 = SHA256.HashData(Encoding.UTF8.GetBytes(user.Password));
            var passwd = Convert.ToHexStringLower(passwd8);

            if (emailCheck && userNameCheck && passwordCheck)
            {
                int lastOrder = await _authDal.GetLastOrderAsync();
                user.Order = lastOrder + 1;
                user.Role = role;
                user.Password = passwd;
                User addUserResult = await _authDal.AddAsync(user);
                return new RegisterUserResult(addUserResult, true);
            }
            else
            {
                if (!emailCheck)
                {
                    return new RegisterUserResult(user, false, "An email can only contain letters, numbers, and certain symbols like '@-._'");
                }
                if (!userNameCheck)
                {
                    return new RegisterUserResult(user, false, "A username can only contain letters, numbers, and certain symbols like '.-_'");
                }
                if (!passwordCheck)
                {
                    return new RegisterUserResult(user, false, "");
                }
                return new RegisterUserResult(user, false, "");
            }
        }

        [GeneratedRegex(@"^[a-zA-Z0-9\s-_]+$")]
        private static partial Regex PasswordRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9\s-_.]+$")]
        private static partial Regex UserNameRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        private static partial Regex EmailRegex();
    }
}

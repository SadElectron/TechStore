using DataAccess.EntityFramework.Abstract;
using Services.Abstract;
using Services.Authentication.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenDal _refreshTokenDal;
        private readonly TokenProvider _tokenProvider;


        public RefreshTokenService(IRefreshTokenDal refreshTokenDal)
        {
            _refreshTokenDal = refreshTokenDal;
        }


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
    }
}



namespace Services.Abstract
{
    public interface IRefreshTokenService
    {
        Task<string> AddTokenAsync(Guid userId);
        Task DeleteTokenAsync(string token);
        Task<bool> ValidateToken(string token, Guid userId);
    }
}
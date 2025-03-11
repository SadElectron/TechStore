using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete;

//Only for demonstration purposes. This service should be implemented with a persistent storage.
public class TokenBlacklistService : ITokenBlacklistService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenBlacklistService> _logger;
    private readonly double _tokenExpirationInMinutes;
    private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

    public TokenBlacklistService(IConfiguration configuration, ILogger<TokenBlacklistService> logger)
    {
        _configuration = configuration;
        _tokenExpirationInMinutes = Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"]);
        _logger = logger;
    }

    public Task AddTokenToBlacklistAsync(string token, DateTime CreatedAt)
    {
        _revokedTokens[token] = CreatedAt;
        return Task.CompletedTask;
    }

    public Task<bool> IsTokenBlacklistedAsync(string token)
    {
        return Task.FromResult(_revokedTokens.TryGetValue(token, out var createdAt));
    }

    public Task RemoveExpiredTokensAsync()
    {
        var startCount = _revokedTokens.Count;
        var now = DateTime.UtcNow;
        foreach (var kvp in _revokedTokens)
        {
            if ((now - kvp.Value).TotalMinutes > _tokenExpirationInMinutes) _revokedTokens.TryRemove(kvp.Key, out _);
        }
        _logger.LogInformation($"Removed {startCount - _revokedTokens.Count} expired tokens from the blacklist.");
        return Task.CompletedTask;
    }
}

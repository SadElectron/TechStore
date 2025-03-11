using Microsoft.Extensions.Hosting;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete;

public class BlacklistRemovalService : BackgroundService
{
    private readonly ITokenBlacklistService _tokenBlacklistService;
    public BlacklistRemovalService(ITokenBlacklistService tokenBlacklistService)
    {
        _tokenBlacklistService = tokenBlacklistService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _tokenBlacklistService.RemoveExpiredTokensAsync();
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // Delay for 10 minutes
        }
    }
}

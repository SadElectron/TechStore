using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract;

public interface ITokenBlacklistService
{
    Task<bool> IsTokenBlacklistedAsync(string token);
    Task AddTokenToBlacklistAsync(string token, DateTime expirationDate);
    Task RemoveExpiredTokensAsync();
}

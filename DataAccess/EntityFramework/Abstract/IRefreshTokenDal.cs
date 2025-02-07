using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IRefreshTokenDal : IEfDbRepository<RefreshToken>
    {
        Task<RefreshToken?> GetWithUserAsync(string refreshToken);
    }
}

using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class RefreshTokenDal : EfDbRepository<RefreshToken, EfDbContext>, IRefreshTokenDal
    {
        public Task<RefreshToken?> GetWithUserAsync(string refreshToken)
        {
            using var context = new EfDbContext();
            return context.RefreshTokens.Where(rt => rt.Token == refreshToken).Include(rt => rt.User).AsNoTracking().SingleOrDefaultAsync();
        }
    }
}

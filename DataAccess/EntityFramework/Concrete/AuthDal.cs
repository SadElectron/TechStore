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
    public class AuthDal : EfDbRepository<User, EfDbContext>, IAuthDal
    {
        public Task<int> GetLastOrderAsync()
        {
            using EfDbContext context = new EfDbContext();
            var order = context.Users.OrderByDescending(u => u.Order).Select(u => u.Order).FirstOrDefaultAsync();
            return order;
        }
    }
}

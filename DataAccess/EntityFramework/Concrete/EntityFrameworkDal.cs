using Core.DataAccess.EntityFramework.Abstract;
using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.EntityFramework.Abstract;
using Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class EntityFrameworkDal : IEntityFrameworkDal
    {
        public Task<int> GetRecordCountAsync<T>() where T : class
        {
            using (var context = new EfDbContext())
            {
                return context.Set<T>().CountAsync();
            }
        }
    }
}

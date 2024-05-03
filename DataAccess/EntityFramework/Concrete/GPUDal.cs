using Core.DataAccess.EntityFramework.Abstract;
using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class GPUDal : EfEntityContextBase<GPU, EfDbContext>, IGPUDal
    {
        
        public Task<int> GetRecordCountAsync()
        {
            using (var context = new EfDbContext())
            {
                return context.GPUs.CountAsync();
            }
        }

        public Task<GPU> GetAsync(Expression<Func<GPU, bool>> filter)
        {
            return base.GetAsync(filter);
        }
    }
}

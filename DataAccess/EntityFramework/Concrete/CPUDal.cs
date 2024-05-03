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
    public class CPUDal : EfEntityContextBase<CPU, EfDbContext>, ICPUDal
    {
        public Task<int> GetRecordCountAsync()
        {
            using (var context = new EfDbContext())
            {
                return context.CPUs.CountAsync();
            }
        }

        //_cpudal.Get(g => g.Id == id);

        public Task<CPU> GetAsync(Expression<Func<CPU, bool>> filter)
        {
            return base.GetAsync(filter);
        }

        public Task<CPU> GetWithImagesAsync(Expression<Func<CPU, bool>> filter)
        {
            using (var context = new EfDbContext())
            {
                return context.CPUs.Include(c => c.Images).SingleOrDefaultAsync(filter);
            }
        }
    }
}

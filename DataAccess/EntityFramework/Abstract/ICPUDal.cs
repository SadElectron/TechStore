using Core.DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface ICPUDal : IEfDbContextBase<CPU>
    {
        public Task<int> GetRecordCountAsync();
        public Task<CPU> GetAsync(Expression<Func<CPU, bool>> filter);

    }
}

using Entities.Concrete;
using Core.DataAccess.EntityFramework.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IGPUDal: IEfDbContextBase<GPU>
    {
        public Task<int> GetRecordCountAsync();
        public Task<GPU> GetAsync(Expression<Func<GPU, bool>> filter);

    }
}

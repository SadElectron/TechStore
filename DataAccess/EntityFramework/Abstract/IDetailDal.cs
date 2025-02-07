using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IDetailDal : IEfDbRepository<Detail>
    {
        Task<Detail> AddOrderedAsync(Detail detail);
        Task<List<Detail>> GetAllWithPropsAsNoTrackingAsync(Guid productId);
        Task<List<Detail>> UpdateDetailsAsync(List<Detail> details);
    }
}

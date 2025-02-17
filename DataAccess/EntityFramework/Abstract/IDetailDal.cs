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
        Task<List<Detail>> GetAllWithPropsAsync(Guid productId);
        Task<List<Detail>> GetByIdsAsync(List<Guid> ids);
        Task<List<Detail>> UpdateDetailsAsync(List<Detail> details);
    }
}

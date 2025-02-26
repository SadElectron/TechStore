using Core.Entities.Concrete;
using Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IDetailService
    {
        Task<EntityCreateResult<Detail>> AddAsync(Detail entity);
        Task<List<Detail>> GetAllAsync(int page, int itemCount);
        Task<Detail?> GetAsync(Guid id);
        Task<Detail?> GetAsync(Expression<Func<Detail, bool>> filter);
        Task<int> GetEntryCountAsync();
        Task<List<Detail>> GetProductDetailsAsync(Guid productId);
        Task<Detail?> GetAsNoTrackingAsync(Guid id);
        Task<EntityUpdateResult<Detail>> UpdateAsync(Detail entity);
        Task<List<Detail>> UpdateDetailsAsync(List<Detail> details);
        Task<EntityDeleteResult> DeleteAsync(Guid id);
        Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id);
        Task<List<Detail>> GetByIdsAsync(List<Guid> ids);
        Task<bool> ExistsAsync(Guid id);
    }
}

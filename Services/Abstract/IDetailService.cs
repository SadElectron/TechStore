using Core.Dtos;
using Core.Entities.Concrete;
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
        Task<Detail> AddAsync(Detail entity);
        Task<List<Detail>> GetAllAsync(int page, int itemCount);
        Task<Detail?> GetAsync(Guid id);
        Task<Detail?> GetAsync(Expression<Func<Detail, bool>> filter);
        Task<Detail> DeleteAsync(Detail entity);
        Task<Detail> UpdateAsync(Detail entity);
        Task<int> GetEntryCountAsync();
        Task<List<Detail>> GetProductDetailsAsync(Guid productId);
        Task<List<Detail>> UpdateDetailsAsync(List<Detail> details);
        Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id);
    }
}

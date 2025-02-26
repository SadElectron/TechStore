using Core.Entities.Abstract;
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
    public interface IPropertyService
    {
        Task<Property> AddAsync(Property entity);
        Task<List<Property>> GetAllAsync(Guid categoryId, int page, int itemCount);
        Task<Property?> GetAsync(Guid id);
        Task<Property?> GetAsync(Expression<Func<Property, bool>> filter);
        Task<Property?> UpdateAsync(Property entity);
        Task<int> GetEntryCountAsync();
        Task<int> GetEntryCountAsync(Guid categoryId);
        Task UpdateAllAsync(List<Property> properties);
        Task<List<Property>> GetAllAsNoTrackingAsync(Guid categoryId, int page, int itemCount);
        Task<List<Property>> GetAllAsNoTrackingAsync(Guid categoryId);
        Task<double> GetLastItemOrder();
        Task<List<Property>> GetProductFilters(Guid categoryId);
        Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<Property, bool>> filter);
        Task<bool> ExistsAsync(Guid id);
    }
}

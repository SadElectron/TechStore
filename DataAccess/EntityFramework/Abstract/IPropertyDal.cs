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
    public interface IPropertyDal : IEfDbRepository<Property>
    {
        Task<Property> AddOrderedAsync(Property property);
        Task<int> DeleteAndReorderAsync(Guid id);
        Task<int> GetLastItemOrder();
        Task<List<Property>> GetProductFilters(Guid categoryId);
        Task<Property?> UpdateAndReorderAsync(Property entity);
    }
}

using Core.DataAccess.EntityFramework.Abstract;
using Core.Dtos;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface ICategoryDal : IEfDbRepository<Category>
    {
        Task<Category> AddOrderedAsync(Category category);
        Task<Category> UpdateAndReorderAsync(Category category);
        Task<int> DeleteAndReorderAsync(Guid id);
        Task<List<Category>> GetFullAsync(int page, int count, int productPage, int productCount);
        Task<int> GetProductCountAsync(Guid categoryId);
        Task<int> GetPropertyCountAsync(Guid categoryId);
    }
}

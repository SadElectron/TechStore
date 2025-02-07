using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface ICategoryService
    {
        Task<Category> AddAsync(Category entity);
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetAllAsync(int page, int itemCount);
        Task<Category?> GetAsync(Guid id);
        Task<Category?> GetAsync(Expression<Func<Category, bool>> filter);
        Task<Category> UpdateAsync(Category entity);
        Task<int> GetEntryCountAsync();
        Task<int> GetProductCountAsync(Guid categoryId);
        Task<int> GetPropertyCount(Guid categoryId);
        Task<int> DeleteAndReorderAsync(Guid id);
        Task<List<Category>> GetFullAsync(int page = 1, int count = 10, int productPage = 1, int productCount = 10);
    }
}

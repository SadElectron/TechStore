using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Concrete;

namespace DataAccess.EntityFramework.Abstract
{
    public interface ICategoryDal : IEfDbRepository<Category>
    {
        Task<List<Category>> GetFullAsync(int page, int count, int productPage, int productCount);
        Task<int> GetProductCountAsync(Guid categoryId);
        Task<int> GetPropertyCountAsync(Guid categoryId);
    }
}

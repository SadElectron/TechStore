using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using Services.Abstract;
using System.Linq.Expressions;

namespace Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly ICategoryDal _categoryDal;

    public CategoryService(ICategoryDal categoryDal)
    {
        _categoryDal = categoryDal;
    }

    public async Task<Category> AddAsync(Category entity)
    {
        var timeNowUtc = DateTimeHelper.GetUtcNow();
        entity.RowOrder = await _categoryDal.GetLastOrderAsync() + 1;
        entity.LastUpdate = timeNowUtc;
        entity.CreatedAt = timeNowUtc;
        return await _categoryDal.AddAsync(entity);
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _categoryDal.GetAllAsNoTrackingAsync(c => c.RowOrder);
    }

    public Task<List<Category>> GetAllAsync(int page, int itemCount)
    {
        return _categoryDal.GetAllAsNoTrackingAsync(page, itemCount, c => c.RowOrder);
    }

    public Task<Category?> GetAsync(Guid id)
    {
        return _categoryDal.GetAsync(t => t.Id == id);
    }
    public Task<Category?> GetAsNoTrackingAsync(Guid id)
    {
        return _categoryDal.GetAsNoTrackingAsync(t => t.Id == id);
    }

    public Task<Category?> GetAsync(Expression<Func<Category, bool>> filter)
    {
        return _categoryDal.GetAsync(filter);
    }

    public Task<int> GetEntryCountAsync()
    {
        return _categoryDal.GetEntryCountAsync();
    }

    public Task<int> GetProductCountAsync(Guid categoryId)
    {
        return _categoryDal.GetProductCountAsync(categoryId);
    }

    public Task<int> GetPropertyCount(Guid categoryId)
    {
        return _categoryDal.GetPropertyCountAsync(categoryId);
    }

    public Task<List<Category>> GetFullAsync(int page = 1, int count = 10, int productPage = 1, int productCount = 10)
    {
        return _categoryDal.GetFullAsync(page, count, productPage, productCount);
    }
    public Task<double> GetLastRowOrder()
    {
        return _categoryDal.GetLastOrderAsync();
    }

    public Task<Category> UpdateAsync(Category entity)
    {
        return _categoryDal.UpdateAsync(entity);
    }

    public async Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id)
    {
        var i = await _categoryDal.DeleteAndReorderAsync(id);
        return i > 0 ? new EntityDeleteResult(true, "Entity deleted") : new EntityDeleteResult(false, "Entity not deleted");
    }
    public async Task<bool> ExistsAsync(Guid id)
    {

        return await _categoryDal.ExistsAsync(id);
    }
    public async Task<bool> ExistsAsync(Expression<Func<Category, bool>> filter)
    {

        return await _categoryDal.ExistsAsync(filter);
    }
}

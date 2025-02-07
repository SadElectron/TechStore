using Core.Dtos;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly ICategoryDal _categoryDal;

    public CategoryService(ICategoryDal categoryDal)
    {
        _categoryDal = categoryDal;
    }

    public Task<Category> AddAsync(Category entity)
    {
        entity.LastUpdate = DateTime.UtcNow;
        return _categoryDal.AddOrderedAsync(entity);
    }

    public Task<int> DeleteAndReorderAsync(Guid id)
    {
        return _categoryDal.DeleteAndReorderAsync(id);
    }

    public Task<Category?> GetAsync(Guid id)
    {
        return _categoryDal.GetAsync(t => t.Id == id);
    }

    public Task<Category?> GetAsync(Expression<Func<Category, bool>> filter)
    {
        return _categoryDal.GetAsync(filter);
    }

    public Task<List<Category>> GetAllAsync(int page, int itemCount)
    {
        return _categoryDal.GetAllAsNoTrackingAsync(page, itemCount, c => c.RowOrder);
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


    public Task<Category> UpdateAsync(Category entity)
    {
        entity.LastUpdate = DateTime.UtcNow;
        return _categoryDal.UpdateAsync(entity);
    }

    public Task<List<Category>> GetFullAsync(int page = 1, int count = 10, int productPage = 1, int productCount = 10)
    {
        return _categoryDal.GetFullAsync(page, count, productPage, productCount);
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _categoryDal.GetAllAsNoTrackingAsync(c => c.RowOrder);
    }
}

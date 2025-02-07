using Core.Entities.Concrete;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete;

public class PropertyService : IPropertyService
{
    private readonly IPropertyDal _propertyDal;

    public PropertyService(IPropertyDal PropertyDal)
    {
        _propertyDal = PropertyDal;
    }

    public Task<Property> AddAsync(Property entity)
    {
        entity.LastUpdate = DateTime.UtcNow;
        return _propertyDal.AddOrderedAsync(entity);
    }

    public Task<int> DeleteAsync(Guid id)
    {
        return _propertyDal.DeleteAndReorderAsync(id);
    }

    public Task<Property?> GetAsync(Guid id)
    {
        return _propertyDal.GetAsync(p => p.Id == id);
    }

    public Task<Property?> GetAsync(Expression<Func<Property, bool>> filter)
    {
        return _propertyDal.GetAsync(filter);
    }

    public Task<List<Property>> GetAllAsync(Guid categoryId, int page, int itemCount)
    {
        return RepoUtils.IsPageAndCountCorrect(page, itemCount) ?
            _propertyDal.GetAllAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.Order) :
            Task.FromResult(new List<Property>());

    }
    public Task<List<Property>> GetAllAsNoTrackingAsync(Guid categoryId, int page, int itemCount)
    {
        return RepoUtils.IsPageAndCountCorrect(page, itemCount) ?
            _propertyDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.Order) :
            Task.FromResult(new List<Property>());

    }
    public Task<List<Property>> GetAllAsNoTrackingAsync(Guid categoryId)
    {
        return _propertyDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, p => p.Order);
    }

    public Task<int> GetEntryCountAsync()
    {
        return _propertyDal.GetEntryCountAsync();
    }

    public async Task<Property?> UpdateAsync(Property entityToUpdate)
    {

        var entity = await _propertyDal.GetAsNoTrackingAsync(p => p.Id == entityToUpdate.Id);


        if (entity == null)
        {
            //log id mismatch
            return null;
        }
        entityToUpdate.CategoryId = entity.CategoryId;
        var dbResult = await _propertyDal.UpdateAndReorderAsync(entityToUpdate);
        if (dbResult == null)
        {
            //Log db fail
        }
        return dbResult;


    }
    public Task UpdateAllAsync(List<Property> properties)
    {
        foreach (var property in properties)
        {
            property.LastUpdate = DateTime.UtcNow;
        }
        return _propertyDal.UpdateAllAsync(properties);
    }

    public Task<int> GetEntryCountAsync(Guid categoryId)
    {
        return _propertyDal.GetEntryCountAsync(p => p.CategoryId == categoryId);
    }
    public Task<int> GetLastItemOrder()
    {
        return _propertyDal.GetLastItemOrder();
    }
    public Task<List<Property>> GetProductFilters(Guid categoryId)
    {
        return _propertyDal.GetProductFilters(categoryId);
    }

}

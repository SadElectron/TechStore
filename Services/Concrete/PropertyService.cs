using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using Services.Abstract;
using System.Linq.Expressions;

namespace Services.Concrete;

public class PropertyService : IPropertyService
{
    private readonly IPropertyDal _propertyDal;
    public PropertyService(IPropertyDal PropertyDal)
    {
        _propertyDal = PropertyDal;
    }
    public async Task<EntityCreateResult<Property>> AddAsync(Property entity)
    {
        var timeNowUtc = DateTimeHelper.GetUtcNow();
        entity.RowOrder = await _propertyDal.GetLastOrderAsync() + 1;
        entity.PropOrder = await _propertyDal.GetLastPropOrderAsync(entity.CategoryId) + 1;
        entity.LastUpdate = timeNowUtc;
        entity.CreatedAt = timeNowUtc;
        return new EntityCreateResult<Property>(true, await _propertyDal.AddAsync(entity));
    }
    public Task<Property?> GetAsync(Guid id)
    {
        return _propertyDal.GetAsync(p => p.Id == id);
    }
    public Task<Property?> GetAsync(Expression<Func<Property, bool>> filter)
    {
        return _propertyDal.GetAsync(filter);
    }
    public Task<Property?> GetAsNoTrackingAsync(Guid id)
    {
        return _propertyDal.GetAsNoTrackingAsync(p => p.Id == id);
    }
    public Task<List<Property>> GetAllAsync(Guid categoryId, int page, int itemCount)
    {
        return _propertyDal.GetAllAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.RowOrder);
    }
    public Task<List<Property>> GetAllAsNoTrackingAsync(Guid categoryId, int page, int itemCount)
    {
        return _propertyDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.RowOrder);
    }
    public Task<List<Property>> GetAllAsNoTrackingAsync(Guid categoryId)
    {
        return _propertyDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, p => p.RowOrder);
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
        var dbResult = await _propertyDal.UpdateAsync(entityToUpdate);
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
    public Task<List<Property>> GetProductFilters(Guid categoryId)
    {
        return _propertyDal.GetProductFilters(categoryId);
    }
    public Task<bool> ExistsAsync(Expression<Func<Property, bool>> filter)
    {
        return _propertyDal.ExistsAsync(filter);
    }
    public Task<bool> ExistsAsync(Guid id)
    {
        return _propertyDal.ExistsAsync(id);
    }
    public Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        return _propertyDal.DeleteAsync(id);
    }
}

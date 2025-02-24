using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Results;
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

public class DetailService : IDetailService
{
    private readonly IDetailDal _detailDal;
    private readonly IPropertyDal _propertyDal;
    private readonly IProductDal _productDal;
    public DetailService(IDetailDal detailDal, IPropertyDal propertyDal, IProductDal productDal)
    {
        _detailDal = detailDal;
        _propertyDal = propertyDal;
        _productDal = productDal;
    }
    public async Task<EntityCreateResult<Detail>> AddAsync(Detail entity)
    {
        var detailProperty = await _propertyDal.GetAsNoTrackingAsync(p => p.Id == entity.PropertyId);
        var detailProduct = await _productDal.GetAsNoTrackingAsync(p => p.Id == entity.ProductId);
        if (detailProperty != null && detailProduct != null)
        {
            entity.RowOrder = await _detailDal.GetLastOrderAsync() + 1;
            entity.LastUpdate = DateTimeHelper.GetUtcNow();
            entity.CreatedAt = DateTimeHelper.GetUtcNow();
            return new EntityCreateResult<Detail>(true, await _detailDal.AddAsync(entity));
        }
        return new EntityCreateResult<Detail>(false, null, "Property or Product not found");
    }



    public Task<Detail?> GetAsync(Guid id)
    {
        return _detailDal.GetAsync(pv => pv.Id == id);

    }
    public Task<Detail?> GetAsNoTrackingAsync(Guid id)
    {
        return _detailDal.GetAsNoTrackingAsync(pv => pv.Id == id);

    }

    public Task<Detail?> GetAsync(Expression<Func<Detail, bool>> filter)
    {
        return _detailDal.GetAsync(filter);

    }

    public Task<List<Detail>> GetAllAsync(int page, int itemCount)
    {
        return _detailDal.GetAllAsync(page, itemCount, d => d.RowOrder);

    }

    public async Task<List<Detail>> GetProductDetailsAsync(Guid productId)
    {
        return await _detailDal.GetAllWithPropsAsync(productId);

    }

    public Task<int> GetEntryCountAsync()
    {
        return _detailDal.GetEntryCountAsync();

    }
    public Task<List<Detail>> GetByIdsAsync(List<Guid> ids)
    {
        return _detailDal.GetByIdsAsync(ids);
    }

    public async Task<EntityUpdateResult<Detail>> UpdateAsync(Detail entity)
    {
        entity.LastUpdate = DateTimeHelper.GetUtcNow();
        var detail = await _detailDal.UpdateAsync(entity);
        return new EntityUpdateResult<Detail>(true, detail);

    }
    public Task<List<Detail>> UpdateDetailsAsync(List<Detail> details)
    {
        
        return _detailDal.UpdateDetailsAsync(details);

    }
    public Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        return _detailDal.DeleteAsync(id);

    }
    public async Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id)
    {
        var entity = await _detailDal.GetAsync(c => c.Id == id);
        if (entity == null) return new EntityDeleteResult(false, "Entity not found");
        var i = await _detailDal.DeleteAndReorderAsync(id);
        return i > 0 ? new EntityDeleteResult(true, "Entity deleted") : new EntityDeleteResult(false, "Entity not deleted");

    }
}

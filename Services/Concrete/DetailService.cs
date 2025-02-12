using Core.Dtos;
using Core.Entities.Abstract;
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

public class DetailService : IDetailService
{
    private readonly IDetailDal _detailDal;
    public DetailService(IDetailDal detailDal)
    {
        _detailDal = detailDal;
    }
    public async Task<Detail> AddAsync(Detail entity)
    {
        entity.RowOrder = await _detailDal.GetLastOrderAsync() + 1;
        entity.LastUpdate = DateTimeHelper.GetUtcNow();
        entity.CreatedAt = DateTimeHelper.GetUtcNow();
        return  await _detailDal.AddAsync(entity);
    }

    public Task<Detail> DeleteAsync(Detail entity)
    {
        return _detailDal.DeleteAsync(entity);

    }
    public async Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id)
    {
        var entity = await _detailDal.GetAsync(c => c.Id == id);

        if (entity == null) return new EntityDeleteResult(false, "Entity not found");

        var i = await _detailDal.DeleteAndReorderAsync(id);
        return i > 0 ? new EntityDeleteResult(true, "Entity deleted") : new EntityDeleteResult(false, "Entity not deleted");

    }

    public Task<Detail?> GetAsync(Guid id)
    {
        return _detailDal.GetAsync(pv => pv.Id == id);

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

    public Task<Detail> UpdateAsync(Detail entity)
    {
        entity.LastUpdate = DateTime.UtcNow;
        return _detailDal.UpdateAsync(entity);

    }
    public Task<List<Detail>> UpdateDetailsAsync(List<Detail> details)
    {
        Parallel.ForEach
            (details, d => d.LastUpdate = DateTime.UtcNow);
        return _detailDal.UpdateDetailsAsync(details);

    }
}

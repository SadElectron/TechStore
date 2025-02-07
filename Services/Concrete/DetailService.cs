using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
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
    public Task<Detail> AddOrderedAsync(Detail entity)
    {
        entity.LastUpdate = DateTime.UtcNow;
        return _detailDal.AddOrderedAsync(entity);
    }

    public Task<Detail> DeleteAsync(Detail entity)
    {
        return _detailDal.DeleteAsync(entity);

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
        return _detailDal.GetAllAsync(page, itemCount, d => d.Order);

    }

    public async Task<List<Detail>> GetProductDetailsAsync(Guid productId)
    {
        return await _detailDal.GetAllWithPropsAsNoTrackingAsync(productId);

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

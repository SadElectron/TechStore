using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Results;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete;

public class PropertyDal : EfDbRepository<Property, EfDbContext>, IPropertyDal
{
    private readonly ILogger<Property> _logger;
    public PropertyDal(ILogger<Property> logger): base(logger)
    {
        _logger = logger;
    }

    public async Task<double> GetLastPropOrderAsync(Guid categoryId)
    {
        using var context = new EfDbContext();
        return await context.Properties.Where(p => p.CategoryId == categoryId).MaxAsync(p => (double?)p.PropOrder) ?? 0;
    }

    public async Task<List<Property>> GetProductFilters(Guid categoryId)
    {
        using var context = new EfDbContext();
        var properties = await context.Properties.Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.RowOrder)
            .Include(p => p.Details)
            .AsNoTracking()
            .ToListAsync();

        var result = properties
            .Select(p => new Property
            {
                PropName = p.PropName,
                Details = p.Details
                    .DistinctBy(d => d.PropValue)
                    .Select(d => new Detail { PropValue = d.PropValue })
                    .ToList()
            })
            .ToList();
        return result;
    }
    public override async Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        using var context = new EfDbContext();
        var transaction = context.Database.BeginTransaction();
        var property = await context.Properties.FindAsync(id);
        try
        {
            var rowOrder = property!.RowOrder;
            var propOrder = property.PropOrder;
            int result = await context.Properties.Where(p => p.Id == property.Id).ExecuteDeleteAsync();
            if (result > 0)
            {
                await context.Properties.Where(p => p.RowOrder > rowOrder).ExecuteUpdateAsync(s => s.SetProperty(p => p.RowOrder, p => p.RowOrder - 1));
                await context.Properties.Where(p => p.PropOrder > propOrder && p.CategoryId == property.CategoryId).ExecuteUpdateAsync(s => s.SetProperty(p => p.PropOrder, p => p.PropOrder - 1));
                await transaction.CommitAsync();
                return new EntityDeleteResult(true, "Property deleted successfully");
            }
            else
            {
                return new EntityDeleteResult(false, "Property not found");
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new EntityDeleteResult(false, $"Something Went Wrong {ex.Message}");
        }
    }

}



using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Concrete;
using Core.Results;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.EntityFramework.Concrete;

public class PropertyDal : EfDbRepository<Property, EfDbContext>, IPropertyDal
{
    private readonly ILogger<Property> _logger;
    private readonly IDetailDal _detailDal;
    public PropertyDal(ILogger<Property> logger, IDetailDal detailDal) : base(logger)
    {
        _logger = logger;
        _detailDal = detailDal;
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
    public async Task<double> GetLastPropOrderByPropertyIdAsync(Guid propertyId)
    {
        using EfDbContext context = new EfDbContext();

        var categoryId = await context.Properties
            .Where(p => p.Id == propertyId)
            .Select(p => p.CategoryId)
            .SingleOrDefaultAsync();
        double? maxProductOrder = await context.Properties
            .Where(p => p.CategoryId == categoryId)
            .MaxAsync(p => (double?)p.PropOrder);

        return maxProductOrder ?? 0;
    }
    public async Task<Property> UpdatePropOrderAsync(Guid propertyId, double newOrder)
    {
        using var context = new EfDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();
        var timeNow = DateTimeHelper.GetUtcNow();
        try
        {
            var property = await context.Properties.Where(p => p.Id == propertyId)
                .Select(p => new { p.Id, p.PropOrder, p.CategoryId })
                .SingleOrDefaultAsync();
            if (property!.PropOrder < newOrder)
            {
                // Shift entities up
                await context.Properties.Where(p => p.PropOrder > property.PropOrder && p.PropOrder <= newOrder && p.CategoryId == property.CategoryId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.PropOrder, p => p.PropOrder - 1));
                await context.Properties.Where(p => p.Id == propertyId).ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.PropOrder, Math.Floor(newOrder))
                    .SetProperty(p => p.LastUpdate, timeNow));
            }
            else if (property.PropOrder > newOrder)
            {
                // Shift entities down
                await context.Properties.Where(p => p.PropOrder < property.PropOrder && p.PropOrder >= newOrder && p.CategoryId == property.CategoryId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.PropOrder, p => p.PropOrder + 1));
                await context.Properties.Where(p => p.Id == propertyId).ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.PropOrder, Math.Ceiling(newOrder))
                    .SetProperty(p => p.LastUpdate, timeNow));
            }

            await transaction.CommitAsync();
            return (await context.Properties.FindAsync(property.Id))!;
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error in PropertyDal.UpdatePropOrderAsync {ex.Message}");
            throw;
        }
    }
    public override async Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        using var context = new EfDbContext();
        var transaction = context.Database.BeginTransaction();
        var property = await context.Properties.Include(p => p.Details).SingleOrDefaultAsync( p => p.Id == id);
        try
        {
            var rowOrder = property!.RowOrder;
            var propOrder = property.PropOrder;
            int result = await context.Properties.Where(p => p.Id == property.Id).ExecuteDeleteAsync();
            if (result > 0)
            {
                await context.Properties.Where(p => p.RowOrder > rowOrder).ExecuteUpdateAsync(s => s.SetProperty(p => p.RowOrder, p => p.RowOrder - 1));
                await context.Properties.Where(p => p.PropOrder > propOrder && p.CategoryId == property.CategoryId).ExecuteUpdateAsync(s => s.SetProperty(p => p.PropOrder, p => p.PropOrder - 1));
                await _detailDal.DeleteRangeAsync(property.Details.ToList());
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
    public async Task<int> DeleteRangeAsync(ICollection<Property> properties)
    {
        if (properties == null || !properties.Any()) return 0;

        using var context = new EfDbContext();
        using var transaction = context.Database.BeginTransaction();
        try
        {
            var firstRowOrder = properties.Min(d => d.RowOrder);
            var ids = properties.Select(d => d.Id).ToList();
            var details = await context.Details.Where(d => ids.Contains(d.PropertyId)).ToListAsync();

            int deletedEntryCount = await context.Properties.Where(d => ids.Contains(d.Id)).ExecuteDeleteAsync();
            if (deletedEntryCount == ids.Count)
            {
                var propertiesGreater = await context.Properties.Where(p => p.RowOrder >= firstRowOrder).ToListAsync();
                double newRowOrder = firstRowOrder;
                foreach (var property in propertiesGreater)
                {
                    property.RowOrder = newRowOrder;
                    newRowOrder++;
                }
                context.Properties.UpdateRange(propertiesGreater);
                await context.SaveChangesAsync();
                await _detailDal.DeleteRangeAsync(details);
                await transaction.CommitAsync();
                return deletedEntryCount;
            }
            await transaction.RollbackAsync();
            return 0;

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error in PropertyDal.DeleteRangeAsync {ex.Message}");
            return 0;
        }

    }

}



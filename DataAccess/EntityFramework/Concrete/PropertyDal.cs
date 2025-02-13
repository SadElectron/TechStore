using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
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
    public async Task<Property> AddOrderedAsync(Property property)
    {
        using EfDbContext context = new EfDbContext();

        Task<double> lastOrder = context.Properties.Where(p => p.CategoryId == property.CategoryId).OrderByDescending(c => c.RowOrder).Select(c => c.RowOrder).FirstOrDefaultAsync();

        property.RowOrder = await lastOrder + 1;

        var addedEntity = await context.Properties.AddAsync(property);
        await context.SaveChangesAsync();

        return addedEntity.Entity;

    }
    public Task<double> GetLastOrderAsync()
    {
        using EfDbContext context = new EfDbContext();
        var lastOrder = context.Properties.OrderByDescending(e => e.RowOrder).Select(e => e.RowOrder).FirstOrDefaultAsync();
        return lastOrder;
    }

    


    public async Task<Property?> UpdateAndReorderAsync(Property entity)
    {
        using var context = new EfDbContext();
        using var transaction = context.Database.BeginTransaction();
        try
        {

            double currentOrder = await context.Properties.Where(p => p.Id == entity.Id).Select(p => p.RowOrder).SingleOrDefaultAsync();

            if (currentOrder < entity.RowOrder)
            {
                // Shift entities up
                await context.Properties
                    .Where(e => e.RowOrder > currentOrder && e.RowOrder <= entity.RowOrder)
                    .OrderBy(e => e.RowOrder)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.RowOrder, p => p.RowOrder - 1));
            }
            else if (currentOrder > entity.RowOrder)
            {
                // Shift entities down
                await context.Properties.Where(e => e.RowOrder < currentOrder && e.RowOrder >= entity.RowOrder)
                    .OrderByDescending(e => e.RowOrder)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.RowOrder, p => p.RowOrder + 1));
            }
            entity.LastUpdate = DateTime.UtcNow;
            context.Properties.Update(entity);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            return entity;
        }
        catch (Exception)
        {

            transaction.Rollback();
            _logger.LogError("Error in PropertyDal.UpdateAndReorderAsync {}");
            throw;
        }
        
    }

    public Task<double> GetLastItemOrder()
    {
        using var context = new EfDbContext();
        return context.Properties.OrderBy(p => p.RowOrder).Select(p => p.RowOrder).LastOrDefaultAsync();

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

    
}



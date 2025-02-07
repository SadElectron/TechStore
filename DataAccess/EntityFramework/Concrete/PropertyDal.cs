using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete;

public class PropertyDal : EfDbRepository<Property, EfDbContext>, IPropertyDal
{

    public async Task<Property> AddOrderedAsync(Property property)
    {
        using EfDbContext context = new EfDbContext();

        Task<int> lastOrder = context.Properties.Where(p => p.CategoryId == property.CategoryId).OrderByDescending(c => c.Order).Select(c => c.Order).FirstOrDefaultAsync();

        property.Order = await lastOrder + 1;

        var addedEntity = await context.Properties.AddAsync(property);
        await context.SaveChangesAsync();

        return addedEntity.Entity;

    }

    public async Task<int> DeleteAndReorderAsync(Guid id)
    {
        using EfDbContext context = new EfDbContext();
        var entity = await context.Properties.Where(p => p.Id == id).SingleOrDefaultAsync();
        if (entity is null)
        {
            return 0;
        }
        int order = entity.Order;

        int deletedEntryCount = await context.Properties.Where(p => p.Id == id).ExecuteDeleteAsync();
        if (deletedEntryCount > 0)
        {
            await context.Properties.Where(p => (p.CategoryId == entity.CategoryId && p.Order > order)).ExecuteUpdateAsync(s => s.SetProperty(p => p.Order, p => p.Order - 1));
        }

        return deletedEntryCount;
    }


    public async Task<Property?> UpdateAndReorderAsync(Property entity)
    {
        using var context = new EfDbContext();
        using var transaction = context.Database.BeginTransaction();
        try
        {

            int currentOrder = await context.Properties.Where(p => p.Id == entity.Id).Select(p => p.Order).SingleOrDefaultAsync();

            if (currentOrder < entity.Order)
            {
                // Shift entities up
                await context.Properties
                    .Where(e => e.Order > currentOrder && e.Order <= entity.Order)
                    .OrderBy(e => e.Order)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Order, p => p.Order - 1));
            }
            else if (currentOrder > entity.Order)
            {
                // Shift entities down
                await context.Properties.Where(e => e.Order < currentOrder && e.Order >= entity.Order)
                    .OrderByDescending(e => e.Order)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Order, p => p.Order + 1));
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
            return null;
        }

    }

    public Task<int> GetLastItemOrder()
    {
        using var context = new EfDbContext();
        return context.Properties.OrderBy(p => p.Order).Select(p => p.Order).LastOrDefaultAsync();

    }

    public async Task<List<Property>> GetProductFilters(Guid categoryId)
    {
        using var context = new EfDbContext();
        var properties = await context.Properties.Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Order)
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



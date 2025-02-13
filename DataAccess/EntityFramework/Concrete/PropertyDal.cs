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



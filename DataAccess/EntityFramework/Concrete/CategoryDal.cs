using Core.DataAccess.EntityFramework.Concrete;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete;

public class CategoryDal : EfDbRepository<Category, EfDbContext>, ICategoryDal
{
    private readonly ILogger<Category> _logger;

    public CategoryDal(ILogger<Category> logger) : base(logger)
    {
        _logger = logger;
    }

    public Task<int> GetProductCountAsync(Guid categoryId)
    {
        using EfDbContext context = new EfDbContext();
        return context.Products.Where(p => p.CategoryId == categoryId).CountAsync();

    }

    public Task<int> GetPropertyCountAsync(Guid categoryId)
    {
        using EfDbContext context = new EfDbContext();
        return context.Properties.Where(p => p.CategoryId == categoryId).CountAsync();

    }

    public async Task<List<Category>> GetFullAsync(int page, int count, int productPage, int productCount)
    {
        using EfDbContext context = new EfDbContext();

        var categories = await context.Categories
            .OrderBy(c => c.RowOrder)
            .Skip((page - 1) * count)
            .Take(count)
            .Select(c => new Category { Id = c.Id, CategoryName = c.CategoryName })
            .AsNoTracking()
            .ToListAsync();

        foreach (var category in categories)
        {
            category.Products = context.Products
                .Where(p => p.CategoryId == category.Id)
                .OrderBy(p => p.RowOrder)
                .Skip((productPage - 1) * productCount)
                .Take(productCount)
                .Include(p => p.Images.OrderBy(i => i.RowOrder))
                .Select(p => new Product { Id = p.Id, ProductName = p.ProductName, Stock = p.Stock, Price = p.Price, Images = p.Images })
                .AsNoTracking()
                .ToList();
        }

        return categories;
    }

    public Task<double> GetLastOrderAsync()
    {
        using EfDbContext context = new EfDbContext();
        var lastOrder = context.Categories.OrderByDescending(u => u.RowOrder).Select(u => u.RowOrder).FirstOrDefaultAsync();
        return lastOrder;
    }

    public async Task<Category> UpdateAndReorderAsync(Category entity)
    {
        using var context = new EfDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            double oldRowOrder = await context.Categories.Where(c => c.Id == entity.Id).Select(c => c.RowOrder).SingleOrDefaultAsync();
            if (oldRowOrder < entity.RowOrder)
            {
                // Shift entities down
                await context.Categories.Where(c => c.RowOrder > oldRowOrder && c.RowOrder <= entity.RowOrder)
                    .ExecuteUpdateAsync(c => c
                        .SetProperty(c => c.RowOrder, c => c.RowOrder - 1)
                        .SetProperty(c => c.LastUpdate, DateTimeHelper.GetUtcNow()));
            }
            else if (oldRowOrder > entity.RowOrder)
            {
                // Shift entities up
                await context.Categories.Where(c => c.RowOrder < oldRowOrder && c.RowOrder >= entity.RowOrder)
                    .ExecuteUpdateAsync(c => c
                        .SetProperty(c => c.RowOrder, c => c.RowOrder + 1)
                        .SetProperty(c => c.LastUpdate, DateTimeHelper.GetUtcNow()));
            }
            entity.LastUpdate = DateTimeHelper.GetUtcNow();
            context.Categories.Update(entity);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            return entity;
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error in CategoryDal.UpdateAndReorderAsync {e.Message}");
            throw;
        }
    }
}

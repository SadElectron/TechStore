using Core.DataAccess.EntityFramework.Concrete;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete;

public class CategoryDal : EfDbRepository<Category, EfDbContext>, ICategoryDal
{

    public Task<int> GetProductCountAsync(Guid categoryId)
    {
        using EfDbContext context = new EfDbContext();
        return context.Products.Where(p => p.CategoryId == categoryId).CountAsync();

    }

    public async Task<Category> AddOrderedAsync(Category category)
    {
        using EfDbContext context = new EfDbContext();

        Task<double> lastOrder = context.Categories.OrderByDescending(c => c.RowOrder).Take(2).Select(c => c.RowOrder).FirstOrDefaultAsync();

        category.RowOrder = await lastOrder + 1;
        category.LastUpdate = DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond);
        category.CreatedAt = DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond);

        var addedEntity = await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return addedEntity.Entity;

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
                        .SetProperty(c => c.LastUpdate, DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond)));
            }
            else if (oldRowOrder > entity.RowOrder)
            {
                // Shift entities up
                await context.Categories.Where(c => c.RowOrder < oldRowOrder && c.RowOrder >= entity.RowOrder)
                    .ExecuteUpdateAsync(c => c
                        .SetProperty(c => c.RowOrder, c => c.RowOrder + 1)
                        .SetProperty(c => c.LastUpdate, DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond)));
            }
            entity.LastUpdate = DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond);
            context.Categories.Update(entity);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            return entity;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<int> DeleteAndReorderAsync(Guid id)
    {
        using EfDbContext context = new EfDbContext();
        var entity = await context.Categories.Where(p => p.Id == id).SingleOrDefaultAsync();
        if (entity is null)
        {
            return 0;
        }
        double order = entity.RowOrder;

        int deletedEntryCount = await context.Categories.Where(p => p.Id == id).ExecuteDeleteAsync();
        if (deletedEntryCount > 0)
        {
            await context.Categories.Where(c => c.RowOrder > order)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.RowOrder, p => p.RowOrder - 1));
        }

        return deletedEntryCount;
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


}

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
}

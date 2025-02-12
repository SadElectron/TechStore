using Core.DataAccess.EntityFramework.Concrete;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.RequestModels;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete;

public class ProductDal : EfDbRepository<Product, EfDbContext>, IProductDal
{
    public Task<CustomerProductDto?> GetFullForCustomer(Guid productId)
    {
        using EfDbContext context = new EfDbContext();
        var product = context.Products.Where(p => p.Id == productId).Select(p => new CustomerProductDto
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Stock = p.Stock,
            Details = p.Details.OrderBy(d => d.RowOrder).Select(d => new CustomerDetailDto
            {
                PropName = d.Property!.PropName,
                PropValue = d.PropValue

            }).ToList(),
            Images = p.Images.OrderBy(i => i.RowOrder).Select(i => new CustomerImageDto { File = i.File }).ToList(),
            Price = p.Price

        }).AsNoTracking().SingleOrDefaultAsync();
        return product;

    }
    public Task<double> GetLastOrderAsync()
    {
        using EfDbContext context = new EfDbContext();
        var lastOrder = context.Products.OrderByDescending(e => e.RowOrder).Select(e => e.RowOrder).FirstOrDefaultAsync();
        return lastOrder;
    }
    
   

    

    public async Task ReorderDb()
    {

        using var context = new EfDbContext();
        int loopOrder = 1;

        foreach (var product in context.Products.OrderBy(p => p.RowOrder))
        {

            product.RowOrder = loopOrder;
            loopOrder++;
        }
        await context.SaveChangesAsync();

    }

    public async Task ReorderCategoryProducts(Guid categoryId)
    {
        using var context = new EfDbContext();

        int loopOrder = 1;
        foreach (var product in context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.RowOrder))
        {
            product.RowOrder = loopOrder;
            loopOrder++;
        }
        await context.SaveChangesAsync();

    }

    public Task<int> GetProductCount(Guid categoryId)
    {

        using var context = new EfDbContext();
        return context.Products.Where(p => p.CategoryId == categoryId).CountAsync();

    }

    public async Task<Product> UpdateAndReorderAsync(Product entity)
    {
        using var context = new EfDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            double oldRowOrder = await context.Products.Where(p => p.Id == entity.Id).Select(p => p.RowOrder).SingleOrDefaultAsync();
            if (oldRowOrder < entity.RowOrder)
            {
                // Shift entities down
                await context.Products.Where(e => e.RowOrder > oldRowOrder && e.RowOrder <= entity.RowOrder)
                    .ExecuteUpdateAsync(p => p.SetProperty(p => p.RowOrder, p => p.RowOrder - 1));
            }
            else if (oldRowOrder > entity.RowOrder)
            {
                // Shift entities up
                await context.Products.Where(e => e.RowOrder < oldRowOrder && e.RowOrder >= entity.RowOrder)
                    .ExecuteUpdateAsync(p => p.SetProperty(p => p.RowOrder, p => p.RowOrder + 1));
            }
            entity.LastUpdate = DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond);
            context.Products.Update(entity);
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

    public Task<List<Product>> GetAllWithImagesAsync(Expression<Func<Product, bool>> filter, int page, int itemCount, Expression<Func<Product, object>> orderFilter)
    {
        using var context = new EfDbContext();
        return context.Products.Where(filter)
            .OrderBy(orderFilter)
            .Include(p => p.Images)
            .Skip((page - 1) * itemCount)
            .Take(itemCount)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetFilteredAsync(List<ProductFilterModel> filters, Guid categoryId, int page = 1, int itemCount = 10)
    {
        using var context = new EfDbContext();
        var query = context.Products.Where(p => p.CategoryId == categoryId).AsQueryable();
        foreach (var filter in filters)
        {
            string filterKey = filter.FilterKey;
            List<string> filterValues = filter.FilterValues;

            query = query.Where(p => p.Details.Any(d => d.Property!.PropName == filterKey && filterValues.Contains(d.PropValue)));

        }
        var products = await query.OrderBy(p => p.RowOrder).Select(p => new Product
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Stock = p.Stock,
            Price = p.Price,
            Details = p.Details.OrderBy(d => d.RowOrder).Select(d => new Detail
            {
                PropValue = d.PropValue,
                Property = new Property { PropName = d.Property!.PropName }
            }).ToList(),
            Images = p.Images.OrderBy(i => i.RowOrder).Select(i => new Image { File = i.File }).ToList()
        }).AsNoTracking()
        .Skip((page - 1) * itemCount)
        .Take(itemCount)
        .ToListAsync();

        return products;
    }
    public async Task<List<Product>> GetFilteredAndSortedAsync(FilterAndSortModel filterAndSortModel, Guid categoryId, int page = 1, int itemCount = 10)
    {
        using var context = new EfDbContext();
        IQueryable<Product> query = context.Products.AsQueryable();
        switch (filterAndSortModel.sort)
        {
            case "date":
                {
                    if (filterAndSortModel.sortValue == "asc")
                    {
                        query = context.Products.Where(p => p.CategoryId == categoryId).OrderByDescending(p => p.CreatedAt).AsQueryable();
                    }
                    else if (filterAndSortModel.sortValue == "desc")
                    {
                        query = context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.CreatedAt).AsQueryable();
                    }

                    break;
                }

            case "price":
                {
                    if (filterAndSortModel.sortValue == "desc")
                    {
                        query = context.Products.Where(p => p.CategoryId == categoryId).OrderByDescending(p => p.Price).AsQueryable();
                    }
                    else if (filterAndSortModel.sortValue == "asc")
                    {
                        query = context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.Price).AsQueryable();
                    }

                    break;
                }

            default:
                {
                    query = context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.RowOrder).AsQueryable();
                    break;
                }
        }


        foreach (var filter in filterAndSortModel.filters)
        {
            string filterKey = filter.FilterKey;
            List<string> filterValues = filter.FilterValues;

            query = query.Where(p => p.Details.Any(d => d.Property!.PropName == filterKey && filterValues.Contains(d.PropValue)));

        }

        var products = await query.Select(p => new Product
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Stock = p.Stock,
            Price = p.Price,
            Details = p.Details.OrderBy(d => d.RowOrder).Select(d => new Detail
            {
                PropValue = d.PropValue,
                Property = new Property { PropName = d.Property!.PropName }
            }).ToList(),
            Images = p.Images.OrderBy(i => i.RowOrder).Select(i => new Image { File = i.File }).ToList()
        }).AsNoTracking()
        .Skip((page - 1) * itemCount)
        .Take(itemCount)
        .ToListAsync();

        return products;
    }
    public Task<int> GetFilteredCountAsync(List<ProductFilterModel> filters, Guid categoryId)
    {
        using var context = new EfDbContext();
        var query = context.Products.Where(p => p.CategoryId == categoryId).AsQueryable();

        foreach (var filter in filters)
        {
            string filterKey = filter.FilterKey;
            List<string> filterValues = filter.FilterValues;
            query = query.Where(p => p.Details.Any(d => d.Property!.PropName == filterKey && filterValues.Contains(d.PropValue)));
        }

        var productCount = query.CountAsync();

        return productCount;
    }
}

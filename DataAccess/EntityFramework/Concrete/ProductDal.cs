using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Concrete;
using Core.RequestModels;
using Core.Results;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;


namespace DataAccess.EntityFramework.Concrete;

public class ProductDal : EfDbRepository<Product, EfDbContext>, IProductDal
{
    private readonly ILogger<Product> _logger;
    public ProductDal(ILogger<Product> logger) : base(logger)
    {
        _logger = logger;
    }
    public async Task<Product?> GetFullForCustomer(Guid productId)
    {
        using EfDbContext context = new EfDbContext();
        var product = await context.Products
            .Include(p => p.Details)
            .ThenInclude(d => d.Property) // Include related Property for Details
            .Include(p => p.Images)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == productId);
        return product;

    }
    public Task<int> GetProductCount(Guid categoryId)
    {

        using var context = new EfDbContext();
        return context.Products.Where(p => p.CategoryId == categoryId).CountAsync();

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
        var products = context.Products.Where(p => p.CategoryId == categoryId).AsQueryable();

        foreach (var filter in filters)
        {
            string filterKey = filter.FilterKey;
            List<string> filterValues = filter.FilterValues;
            products = products.Where(p => p.Details.Any(d => d.Property!.PropName == filterKey && filterValues.Contains(d.PropValue)));
        }

        var productCount = products.CountAsync();

        return productCount;
    }
    public async Task<double> GetLastProductOrderByProductIdAsync(Guid productId)
    {
        using EfDbContext context = new EfDbContext();

        var categoryId = await context.Products
            .Where(p => p.Id == productId)
            .Select(p => p.CategoryId)
            .SingleOrDefaultAsync();
        double? maxProductOrder = await context.Products
            .Where(p => p.CategoryId == categoryId)
            .MaxAsync(p => (double?)p.ProductOrder);

        return maxProductOrder ?? 0;
    }
    public async Task<double> GetLastProductOrderByCategoryIdAsync(Guid categoryId)
    {
        using EfDbContext context = new EfDbContext();
        return await context.Products.Where(p => p.CategoryId == categoryId).MaxAsync(p => (double?)p.ProductOrder) ?? 0;
    }
    public async Task<Product> UpdateProductOrderAsync(Guid productId, double newOrder)
    {
        using var context = new EfDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();
        var timeNow = DateTimeHelper.GetUtcNow();
        try
        {
            var product = await context.Products.Where(p => p.Id == productId)
                .Select(p => new { p.Id, p.ProductOrder, p.CategoryId })
                .SingleOrDefaultAsync();
            if (product!.ProductOrder < newOrder)
            {
                // Shift entities up
                await context.Products.Where(p => p.ProductOrder > product.ProductOrder && p.ProductOrder <= newOrder && p.CategoryId == product.CategoryId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.ProductOrder, p => p.ProductOrder - 1));
                await context.Products.Where(p => p.Id == productId).ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.ProductOrder, Math.Floor(newOrder))
                    .SetProperty(p => p.LastUpdate, timeNow));
            }
            else if (product.ProductOrder > newOrder)
            {
                // Shift entities down
                await context.Products.Where(p => p.ProductOrder < product.ProductOrder && p.ProductOrder >= newOrder && p.CategoryId == product.CategoryId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.ProductOrder, p => p.ProductOrder + 1));
                await context.Products.Where(p => p.Id == productId).ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.ProductOrder, Math.Ceiling(newOrder))
                    .SetProperty(p => p.LastUpdate, timeNow));
            }

            await transaction.CommitAsync();
            return (await context.Products.FindAsync(product.Id))!;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public override async Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        using var context = new EfDbContext();
        var transaction = context.Database.BeginTransaction();
        var product = await context.Products.FindAsync(id);
        try
        {
            var rowOrder = product!.RowOrder;
            var productOrder = product.ProductOrder;
            int result = await context.Products.Where(p => p.Id == product.Id).ExecuteDeleteAsync();
            if (result > 0)
            {
                await context.Products.Where(p => p.RowOrder > rowOrder).ExecuteUpdateAsync(s => s.SetProperty(p => p.RowOrder, p => p.RowOrder - 1));
                await context.Products.Where(p => p.ProductOrder > productOrder && p.CategoryId == product.CategoryId).ExecuteUpdateAsync(s => s.SetProperty(p => p.ProductOrder, p => p.ProductOrder - 1));
                await transaction.CommitAsync();
                return new EntityDeleteResult(true, "Product deleted successfully");
            }
            else
            {
                return new EntityDeleteResult(false, "Product not found");
            }


        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new EntityDeleteResult(false, $"Something Went Wrong {ex.Message}");
        }
    }
}

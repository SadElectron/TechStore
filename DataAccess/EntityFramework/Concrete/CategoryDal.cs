using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Concrete;
using Core.Results;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.EntityFramework.Concrete;

public class CategoryDal : EfDbRepository<Category, EfDbContext>, ICategoryDal
{
    private readonly ILogger<Category> _logger;
    private readonly IProductDal _productDal;
    private readonly IPropertyDal _propertyDal;

    public CategoryDal(ILogger<Category> logger, IPropertyDal propertyDal, IProductDal productDal) : base(logger)
    {
        _logger = logger;
        _propertyDal = propertyDal;
        _productDal = productDal;
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
    public override async Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        using var context = new EfDbContext();
        using var transaction = context.Database.BeginTransaction();
        try
        {
            double rowOrder = await context.Categories.Where(e => e.Id == id).Select(e => e.RowOrder).SingleOrDefaultAsync();
            var category = await context.Categories.Include(c => c.Products).Include(c => c.Properties).AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            int deletedEntryCount = await context.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
            if (deletedEntryCount > 0)
            {
                await context.Categories.Where(c => c.RowOrder > rowOrder).ExecuteUpdateAsync(s => s.SetProperty(c => c.RowOrder, p => p.RowOrder - 1));
                await _productDal.DeleteRangeAsync(category.Products);
                await _propertyDal.DeleteRangeAsync(category.Properties);
                await transaction.CommitAsync();
                return new EntityDeleteResult(true, "Entity deleted successfully");
            }
            await transaction.RollbackAsync();
            return new EntityDeleteResult(false, "Entity not found");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Error in CategoryDal.DeleteAsync {ex.Message}");
            return new EntityDeleteResult(false, "Failed to delete entity");
        }
        
       

        
    }
}

using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Concrete;
using Core.RequestModels;
using System.Linq.Expressions;


namespace DataAccess.EntityFramework.Abstract;

public interface IProductDal : IEfDbRepository<Product>
{
    Task<Product?> GetFullForCustomer(Guid productId);
    Task<int> GetProductCount(Guid categoryId);
    Task<List<Product>> GetAllWithImagesAsync(Expression<Func<Product, bool>> filter, int page, int itemCount, Expression<Func<Product, object>> orderFilter);
    Task<List<Product>> GetFilteredAsync(List<ProductFilterModel> filters, Guid categoryId, int page = 1, int itemCount = 10);
    Task<List<Product>> GetFilteredAndSortedAsync(FilterAndSortModel filterAndSortModel, Guid categoryId, int page = 1, int itemCount = 10);
    Task<int> GetFilteredCountAsync(List<ProductFilterModel> filters, Guid categoryId);
    Task<double> GetLastProductOrderByProductIdAsync(Guid productId);
    Task<double> GetLastProductOrderByCategoryIdAsync(Guid categoryId);
    Task<Product> UpdateProductOrderAsync(Guid productId, double newOrder);
}

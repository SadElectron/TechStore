using Core.Dtos;
using Core.Entities.Concrete;
using Core.RequestModels;
using Core.Results;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Services.Abstract
{
    public interface IProductService
    {
        Task<Product> GetAsync(Guid id);
        Task<Product?> GetAsync(Expression<Func<Product, bool>> filter);
        Task<Product?> GetAsNoTrackingAsync(Guid id);
        Task<List<Product>> GetAllAsync(int page, int itemCount);
        Task<List<Product>> GetAllAsNoTrackingAsync(int page, int itemCount, Guid categoryId);
        Task<int> GetEntryCountAsync();
        Task<int> GetProductCountAsync(Guid categoryId);
        Task<CustomerProductDto?> GetFullForCustomer(Guid productId);
        Task<List<Product>> GetAllWithImagesAsync(int page, int itemCount, Guid categoryId);
        Task<List<Product>> GetFilteredAsync(List<ProductFilterModel> filters, Guid categoryId, int page = 1, int itemCount = 10);
        Task<List<Product>> GetFilteredAndSortedAsync(FilterAndSortModel filterAndSortModel, Guid categoryId, int page = 1, int itemCount = 10);
        Task<int> GetFilteredCountAsync(List<ProductFilterModel> filters, Guid categoryId);
        Task<List<Product>> GetAllAsNoTrackingAsync(string q);
        Task<EntityCreateResult<Product>> AddAsync(Product entity);
        Task<Product> UpdateAsync(Product entity);
        Task<EntityDeleteResult> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid productId);
        Task<double> GetLastProductOrderByCategoryIdAsync(Guid categoryId);
        Task<double> GetLastProductOrderByProductIdAsync(Guid productId);
        Task<Product> UpdateProductOrderAsync(Guid id, double newOrder);
    }
}

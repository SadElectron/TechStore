using Core.Dtos;
using Core.Entities.Concrete;
using Core.RequestModels;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Services.Abstract;
using System.Linq.Expressions;
using Core.Utils;
using Core.Results;

namespace Services.Concrete;

public class ProductService : IProductService
{
    private readonly IProductDal _productDal;
    public ProductService(IProductDal productDal)
    {
        _productDal = productDal;
    }
    public async Task<Product> GetAsync(Guid id)
    {
        var entity = await _productDal.GetAsNoTrackingAsync(p => p.Id == id);
        return entity!;
    }
    public Task<Product?> GetAsync(Expression<Func<Product, bool>> filter)
    {
        return _productDal.GetAsync(filter);
    }
    public Task<List<Product>> GetAllAsync(int page, int itemCount)
    {
        return _productDal.GetAllAsNoTrackingAsync(page, itemCount, p => p.RowOrder);
    }
    public Task<List<Product>> GetAllAsync(int page, int itemCount, Guid categoryId)
    {
        return _productDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.RowOrder);
    }
    public Task<int> GetEntryCountAsync()
    {
        return _productDal.GetEntryCountAsync();
    }
    public Task<int> GetProductCountAsync(Guid categoryId)
    {
        return _productDal.GetProductCount(categoryId);
    }
    public Task<Product?> GetAsNoTrackingAsync(Guid id)
    {
        return _productDal.GetAsNoTrackingAsync(p => p.Id == id);
    }
    public Task<CustomerProductDto?> GetFullForCustomer(Guid productId)
    {
        return _productDal.GetFullForCustomer(productId);
    }
    public Task<List<Product>> GetAllAsNoTrackingAsync(int page, int itemCount, Guid categoryId)
    {
        return _productDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.RowOrder);
    }
    public Task<List<Product>> GetAllWithImagesAsync(int page, int itemCount, Guid categoryId)
    {
        return _productDal.GetAllWithImagesAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.RowOrder);
    }
    public Task<List<Product>> GetFilteredAsync(List<ProductFilterModel> filters, Guid categoryId, int page = 1, int itemCount = 10)
    {
        return _productDal.GetFilteredAsync(filters, categoryId, page, itemCount);
    }
    public Task<List<Product>> GetFilteredAndSortedAsync(FilterAndSortModel filterAndSortModel, Guid categoryId, int page = 1, int itemCount = 10)
    {
        return _productDal.GetFilteredAndSortedAsync(filterAndSortModel, categoryId, page, itemCount);
    }
    public Task<int> GetFilteredCountAsync(List<ProductFilterModel> filters, Guid categoryId)
    {
        return _productDal.GetFilteredCountAsync(filters, categoryId);
    }
    public Task<List<Product>> GetAllAsNoTrackingAsync(string q)
    {
        return _productDal.GetAllAsNoTrackingAsync(p => EF.Functions.Like(p.ProductName, $"%{q}%"), p => p.RowOrder);
    }
    public Task<double> GetLastProductOrderByProductIdAsync(Guid productId)
    {
        return _productDal.GetLastProductOrderByProductIdAsync(productId);
    }
    public Task<double> GetLastProductOrderByCategoryIdAsync(Guid categoryId)
    {
        return _productDal.GetLastProductOrderByCategoryIdAsync(categoryId);
    }
    public Task<bool> ExistsAsync(Guid productId)
    {
        return _productDal.ExistsAsync(productId);
    }
    public async Task<EntityCreateResult<Product>> AddAsync(Product entity)
    {
        var timeNowUtc = DateTimeHelper.GetUtcNow();
        entity.ProductOrder = await _productDal.GetLastProductOrderByCategoryIdAsync(entity.CategoryId) + 1;
        entity.RowOrder = await _productDal.GetLastOrderAsync() + 1;
        entity.SoldQuantity = 0;
        entity.LastUpdate = timeNowUtc;
        entity.CreatedAt = timeNowUtc;
        var addedEntity = await _productDal.AddAsync(entity);
        return new EntityCreateResult<Product>(true, addedEntity);

    }
    public async Task<Product> UpdateAsync(Product entity)
    {
        return await _productDal.UpdateAsync(entity);
    }
    public Task<Product> UpdateProductOrderAsync(Guid id, double newOrder)
    {
        return _productDal.UpdateProductOrderAsync(id, newOrder);
    }
    public Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        return _productDal.DeleteAsync(id);
    }
}

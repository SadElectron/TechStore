using AutoMapper;
using Core.Dtos;
using Core.Entities.Concrete;
using Core.RequestModels;
using DataAccess.EntityFramework.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Abstract;
using System.Linq.Expressions;
using Core.Utils;
using DataAccess.EntityFramework.Concrete;
using Core.Results;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Services.Concrete;

public class ProductService : IProductService
{
    private readonly IProductDal _productDal;
    private readonly IImageDal _imageDal;
    private readonly IDetailDal _detailDal;
    private readonly ICategoryDal _categoryDal;
    private readonly IMapper _mapper;

    public ProductService(IProductDal productDal, IMapper mapper, IImageDal imageDal, IDetailDal detailDal, ICategoryDal categoryDal)
    {
        _productDal = productDal;
        _mapper = mapper;
        _imageDal = imageDal;
        _detailDal = detailDal;
        _categoryDal = categoryDal;
    }
    public async Task<Product> GetAsync(Guid id)
    {

        var entity = await _productDal.GetAsNoTrackingAsync(p => p.Id == id);
        return entity ?? new Product { ProductName = "" };

    }
    public Task<Product?> GetAsync(Expression<Func<Product, bool>> filter)
    {
        return _productDal.GetAsync(filter);
    }
    public Task<List<Product>> GetAllAsync(int page, int itemCount)
    {
        // Validate parameters
        if (page <= 0 || itemCount <= 0)
            return Task.FromResult<List<Product>>(new());

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
    public Task<double> GetLastProductOrderAsync()
    {
        return _productDal.GetLastProductOrderAsync();
    }
    public Task<bool> ExistsAsync(Guid productId)
    {
        return _productDal.ExistsAsync(productId);
    }
    public async Task<EntityCreateResult<Product>> AddAsync(Product entity)
    {

        entity.ProductOrder = await _productDal.GetLastProductOrderAsync() + 1;
        entity.RowOrder = await _productDal.GetLastOrderAsync() + 1;
        entity.SoldQuantity = 0;
        entity.LastUpdate = DateTimeHelper.GetUtcNow();
        entity.CreatedAt = DateTimeHelper.GetUtcNow();
        var addedEntity = await _productDal.AddAsync(entity);
        return new EntityCreateResult<Product>(true, addedEntity);

    }
    public async Task<Product> UpdateAsync(Product entity)
    {
        var oldEntity = await _productDal.GetAsNoTrackingAsync(p => p.Id == entity.Id);
        
        return await _productDal.UpdateAsync(entity);
    }
    public async Task<Product> UpdateProductOrderAsync(Guid id, double newOrder)
    {
        return await _productDal.UpdateProductOrderAsync(id, newOrder);
    }
    public Task<EntityDeleteResult> DeleteAsync(Guid id)
    {
        return _productDal.DeleteAsync(id);
    }
}

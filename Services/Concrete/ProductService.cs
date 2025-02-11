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

namespace Services.Concrete;

public class ProductService : IProductService
{
    private readonly IProductDal _productDal;
    private readonly IImageDal _imageDal;
    private readonly IDetailDal _detailDal;
    private readonly IMapper _mapper;

    public ProductService(IProductDal productDal, IMapper mapper, IImageDal imageDal, IDetailDal detailDal)
    {
        _productDal = productDal;
        _mapper = mapper;
        _imageDal = imageDal;
        _detailDal = detailDal;
    }

    public async Task<Product> AddAsync(Product entity)
    {
        entity.LastUpdate = DateTimeHelper.GetUtcNow();
        entity.CreatedAt = DateTimeHelper.GetUtcNow();
        entity.RowOrder = await _productDal.GetLastOrderAsync() + 1;

        if (entity.Details != null)
        {
            foreach (var detail in entity.Details)
            {
                detail.LastUpdate = DateTime.UtcNow;
            }
        }

        return await _productDal.AddAsync(entity);
    }
    public async Task<Product> AddAsync(Product entity, List<IFormFile> imageList)
    {

        //control image length and extension

        var productId = Guid.NewGuid();
        var imageRowOrder = await _imageDal.GetLastOrderAsync();
        var imageOrder = 0;
        entity.Images = new List<Image>();
        foreach (var image in imageList)
        {

            imageRowOrder++;
            imageOrder++;
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            entity.Images.Add(new Image
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    ImageOrder = imageOrder,
                    RowOrder = imageRowOrder,
                    File = memoryStream.ToArray(),
                    LastUpdate = DateTimeHelper.GetUtcNow(),
                    CreatedAt = DateTimeHelper.GetUtcNow()
                    
                });
        }

        if (entity.Details != null)
        {
            var detailRowOrder = await _detailDal.GetLastOrderAsync();
            foreach (var detail in entity.Details)
            {
                detailRowOrder++;
                detail.Id = Guid.NewGuid();
                detail.ProductId = productId;
                detail.LastUpdate = DateTimeHelper.GetUtcNow();
                detail.CreatedAt = DateTimeHelper.GetUtcNow();
                detail.RowOrder = detailRowOrder;
            }
        }
        entity.LastUpdate = DateTimeHelper.GetUtcNow();
        entity.CreatedAt = DateTimeHelper.GetUtcNow();
        entity.Id = productId;
        entity.RowOrder = await _productDal.GetLastOrderAsync() + 1;
        return await _productDal.AddOrderedAsync(entity);

    }

    public Task<int> DeleteAndReorderAsync(Guid id)
    {
        return _productDal.DeleteAndReorderAsync(id);
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

    public async Task<Product> UpdateAndReorderAsync(Product entity)
    {

        var entityToUpdate = await _productDal.GetAsNoTrackingAsync(p => p.Id == entity.Id);
        if (entityToUpdate != null) // update and reorder
        {
            return await _productDal.UpdateAndReorderAsync(entity);

        }
        else if (entityToUpdate == null) //No entry
        {
            return new Product { ProductName = string.Empty };
        }
        else //bad request
        {
            return new Product { ProductName = string.Empty };
        }
    }

    public async Task ReorderDb()
    {
        await _productDal.ReorderDb();
    }
    public async Task ReorderCategoryProducts(Guid categoryId)
    {
        await _productDal.ReorderCategoryProducts(categoryId);
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
}

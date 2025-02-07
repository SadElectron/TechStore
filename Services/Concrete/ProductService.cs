using AutoMapper;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.RequestModels;
using DataAccess.EntityFramework.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete;

public class ProductService : IProductService
{
    private readonly IProductDal _productDal;
    private readonly IMapper _mapper;

    public ProductService(IProductDal productDal, IMapper mapper)
    {
        _productDal = productDal;
        _mapper = mapper;
    }

    public Task<Product> AddAsync(Product entity)
    {
        entity.LastUpdate = DateTime.UtcNow;


        if (entity.Details != null)
        {
            foreach (var detail in entity.Details)
            {
                detail.LastUpdate = DateTime.UtcNow;
            }
        }

        return _productDal.AddAsync(entity);
    }
    public async Task<Product> AddWithImagesAsync(Product entity, List<IFormFile> imageList)
    {
        //control image length and extension

        var productId = Guid.NewGuid();
        var productCount = _productDal.GetProductCount(entity.CategoryId);

        var order = 0;
        entity.Images = new List<Image>();
        foreach (var image in imageList)
        {
            order += 1;
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            entity.Images.Add(new Image
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Order = order,
                File = memoryStream.ToArray()
            });
        }

        if (entity.Details != null)
        {
            foreach (var detail in entity.Details)
            {
                detail.Id = Guid.NewGuid();
                detail.ProductId = productId;
                detail.LastUpdate = DateTime.UtcNow;
            }
        }
        entity.LastUpdate = DateTime.UtcNow;
        entity.Id = productId;
        entity.Order = (await productCount) + 1;
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
        return _productDal.GetAllAsNoTrackingAsync(page, itemCount, p => p.Order);
    }
    public Task<List<Product>> GetAllAsync(int page, int itemCount, Guid categoryId)
    {
        return _productDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.Order);
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
        return _productDal.GetAllAsNoTrackingAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.Order);
    }
    public Task<List<Product>> GetAllWithImagesAsync(int page, int itemCount, Guid categoryId)
    {
        return _productDal.GetAllWithImagesAsync(p => p.CategoryId == categoryId, page, itemCount, p => p.Order);
    }
    public Task<List<Product>> GetFilteredAsync(List<ProductFilterModel> filters, Guid categoryId, int page = 1, int itemCount = 10)
    {
        return _productDal.GetFilteredAsync(filters, categoryId, page, itemCount);
    }
    public Task<List<Product>> GetFilteredAndSortedAsync(FilterAndSortModel filterAndSortModel, Guid categoryId, int page = 1, int itemCount = 10)
    {
        return _productDal.GetFilteredAndSortedAsync(filterAndSortModel,categoryId, page, itemCount);
    }
    public Task<int> GetFilteredCountAsync(List<ProductFilterModel> filters, Guid categoryId)
    {
        return _productDal.GetFilteredCountAsync(filters, categoryId);
    }

    public Task<List<Product>> GetAllAsNoTrackingAsync(string q)
    {
        return _productDal.GetAllAsNoTrackingAsync(p => EF.Functions.Like(p.ProductName, $"%{q}%"), p => p.Order);
    }
}

using Core.DataAccess.EntityFramework.Abstract;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IProductDal : IEfDbRepository<Product>
    {
        Task<CustomerProductDto?> GetFullForCustomer(Guid productId);
        Task<int> GetProductCount(Guid categoryId);
        Task ReorderCategoryProducts(Guid categoryId);
        Task ReorderDb();
        Task<List<Product>> GetAllWithImagesAsync(Expression<Func<Product, bool>> filter, int page, int itemCount, Expression<Func<Product, object>> orderFilter);
        Task<List<Product>> GetFilteredAsync(List<ProductFilterModel> filters, Guid categoryId, int page = 1, int itemCount = 10);
        Task<List<Product>> GetFilteredAndSortedAsync(FilterAndSortModel filterAndSortModel, Guid categoryId, int page = 1, int itemCount = 10);
        Task<int> GetFilteredCountAsync(List<ProductFilterModel> filters, Guid categoryId);
    }
}

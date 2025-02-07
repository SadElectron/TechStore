using Core.DataAccess.EntityFramework.Concrete;
using Core.Dtos;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class CategoryDal : EfDbRepository<Category, EfDbContext>, ICategoryDal
    {

        public Task<int> GetProductCountAsync(Guid categoryId)
        {
            using EfDbContext context = new EfDbContext();
            return context.Products.Where(p => p.CategoryId == categoryId).CountAsync();

        }

        public async Task<Category> AddOrderedAsync(Category category)
        {
            using EfDbContext context = new EfDbContext();

            Task<double> lastOrder = context.Categories.OrderByDescending(c => c.RowOrder).Select(c => c.RowOrder).FirstOrDefaultAsync();

            category.RowOrder = await lastOrder + 1;

            var addedEntity = await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return addedEntity.Entity;

        }
        public async Task<int> DeleteAndReorderAsync(Guid id)
        {
            using EfDbContext context = new EfDbContext();
            var entity = await context.Categories.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (entity is null)
            {
                return 0;
            }
            double order = entity.RowOrder;

            int deletedEntryCount = await context.Categories.Where(p => p.Id == id).ExecuteDeleteAsync();
            if (deletedEntryCount > 0)
            {
                await context.Categories.Where(c => (c.RowOrder > order)).ExecuteUpdateAsync(s => s.SetProperty(c => c.RowOrder, p => p.RowOrder - 1));
            }

            return deletedEntryCount;
        }

        public Task<int> GetPropertyCountAsync(Guid categoryId)
        {
            using EfDbContext context = new EfDbContext();
            return context.Properties.Where(p => p.CategoryId == categoryId).CountAsync();

        }

        public async Task<List<Category>> GetFullAsync(int page, int count, int productPage, int productCount)
        {
            using EfDbContext context = new EfDbContext();

            // Not supported in sqlite
            /*var taskList = context.Categories.Skip((page - 1) * count).Take(count).Select(c => new CustomerCategoryDto
            {
                CategoryName = c.CategoryName,
                Id = c.Id,
                Products = context.Products.Where(p => p.CategoryId == c.Id).Skip((productPage - 1) * productCount).Take(productCount).Select(p => new CustomerProductDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Stock = p.Stock,
                    Price = p.Price,
                    Details = new List<CustomerDetailDto>(),
                    Images = context.Images.Where(i => i.ProductId == p.Id).OrderBy(i => i.Order).Select(i => new CustomerImageDto
                    {
                        File = i.File
                    }).ToList(),
                }).ToList()
            }).ToListAsync();*/

            var categories = await context.Categories
                .OrderBy(c => c.RowOrder)
                .Skip((page - 1) * count)
                .Take(count)
                .Select(c => new Category { Id = c.Id, CategoryName = c.CategoryName})
                .AsNoTracking()
                .ToListAsync();
            

            foreach (var category in categories) 
            {
                category.Products =  context.Products
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
    }
}

﻿using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class ImageDal : EfDbRepository<Image, EfDbContext>, IImageDal
    {
        public ImageDal()
        {

        }
        public Task<double> GetLastOrderAsync()
        {
            using EfDbContext context = new EfDbContext();
            var lastOrder = context.Images.OrderByDescending(e => e.RowOrder).Select(e => e.RowOrder).FirstOrDefaultAsync();
            return lastOrder;
        }
        public Task<double> GetLastImageOrderAsync(Expression<Func<Image, bool>> filter)
        {
            using EfDbContext context = new EfDbContext();
            var lastImageOrder = context.Images.Where(filter).OrderByDescending(e => e.ImageOrder).Select(e => e.ImageOrder).FirstOrDefaultAsync();
            return lastImageOrder;
        }
        public async Task<IEnumerable<Image>> AddAllAsync(ICollection<Image> images)
        {
            using var context = new EfDbContext();
            double lastOrder = await context.Images.OrderByDescending(i => i.RowOrder)
                .Take(2)
                .Select(i => i.RowOrder)
                .FirstOrDefaultAsync();
            double lastImageOrder = await context.Images.Where(i => i.ProductId == images.First().ProductId)
                .OrderByDescending(i => i.ImageOrder)
                .Take(2)
                .Select(i => i.ImageOrder)
                .FirstOrDefaultAsync();

            foreach (var image in images)
            {
                lastOrder += 1;
                image.RowOrder = lastOrder;
                if (image.ImageOrder == 0)
                {
                    lastImageOrder++;
                    image.ImageOrder = lastImageOrder;
                }
                image.LastUpdate = DateTime.UtcNow;
            }
            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();
            var addedImages = await context.Images
                .Where(i => i.ProductId == images.First().ProductId)
                .OrderBy(i => i.ImageOrder)
                .AsNoTracking()
                .ToListAsync();

            return addedImages;
        }
        public Task<int> DeleteImagesAsync(Guid productId)
        {
            using var context = new EfDbContext();
            return context.Images.Where(i => i.ProductId == productId).ExecuteDeleteAsync();

        }

        
        
        public async Task<Image?> UpdateOrderAsync(Guid imageId, int newOrder)
        {
            using var context = new EfDbContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var image = await context.Images.Where(i => i.Id == imageId).SingleOrDefaultAsync();
                if (image is null) return null;
                if (image.RowOrder < newOrder)
                {
                    // Shift entities up
                    await context.Images.Where(i => i.RowOrder > image.RowOrder && i.RowOrder <= newOrder && i.ProductId == image.ProductId)
                        .OrderBy(e => e.RowOrder)
                        .ExecuteUpdateAsync(s => s.SetProperty(i => i.RowOrder, i => i.RowOrder - 1));


                }
                else if (image.RowOrder > newOrder)
                {
                    // Shift entities down
                    await context.Images.Where(i => i.RowOrder < image.RowOrder && i.RowOrder >= newOrder && i.ProductId == image.ProductId)
                        .OrderByDescending(e => e.RowOrder)
                        .ExecuteUpdateAsync(s => s.SetProperty(i => i.RowOrder, i => i.RowOrder + 1));

                }
                image.LastUpdate = DateTime.UtcNow;
                image.RowOrder = newOrder;
                var updateResult = context.Images.Update(image);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return updateResult.Entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                //log fail
                throw;
            }
        }
    }
}

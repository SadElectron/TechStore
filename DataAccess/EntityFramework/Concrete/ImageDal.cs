using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Utils;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<Image> _logger;
        public ImageDal(ILogger<Image> logger) : base(logger)
        {
            _logger = logger;
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
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                double lastOrder = await context.Images.OrderByDescending(i => i.RowOrder)
                .Select(i => i.RowOrder)
                .FirstOrDefaultAsync();
                double lastImageOrder = await context.Images.Where(i => i.ProductId == images.First().ProductId)
                    .OrderByDescending(i => i.ImageOrder)
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
                    image.LastUpdate = DateTimeHelper.GetUtcNow();
                }
                await context.Images.AddRangeAsync(images);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError($"Error in ImageDal.AddAllAsync {e.Message}");
            }
            
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
                image.LastUpdate = DateTimeHelper.GetUtcNow();
                image.RowOrder = newOrder;
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return image;
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

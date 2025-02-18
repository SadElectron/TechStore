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
            if (images == null || !images.Any())
            {
                throw new ArgumentException("No images provided.", nameof(images));
            }
            using var context = new EfDbContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var productId = images.First().ProductId;

                // Fetch the last RowOrder for the entire table
                double lastOrder = await context.Images
                    .OrderByDescending(i => i.RowOrder)
                    .Select(i => i.RowOrder)
                    .FirstOrDefaultAsync();

                // Fetch the last ImageOrder for the specific product
                double lastImageOrder = await context.Images
                    .Where(i => i.ProductId == productId)
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
                    image.CreatedAt = DateTimeHelper.GetUtcNow();
                }
                await context.Images.AddRangeAsync(images);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return images;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError($"Error in ImageDal.AddAllAsync {e.Message}");
                throw;
            }
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
        public new async Task<int> DeleteAndReorderAsync(Guid id)
        {
            using var context = new EfDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var image = await context.Images
                    .Where(e => e.Id == id)
                    .Select(e => new { e.RowOrder, e.ImageOrder })
                    .SingleOrDefaultAsync();

                if (image == null)
                {
                    return 0; // Image not found, nothing to delete
                }

                int deletedEntryCount = await context.Images.Where(p => p.Id == id).ExecuteDeleteAsync();
                if (deletedEntryCount > 0)
                {
                    await context.Images.Where(e => e.RowOrder > image.RowOrder)
                            .ExecuteUpdateAsync(s => s.SetProperty(e => e.RowOrder, e => e.RowOrder - 1));
                    await context.Images.Where(e => e.ImageOrder > image.ImageOrder)
                            .ExecuteUpdateAsync(s => s.SetProperty(e => e.ImageOrder, e => e.ImageOrder - 1));
                }

                await transaction.CommitAsync();

                return deletedEntryCount;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while deleting and reordering images: {Message}", ex.Message);
                throw;
            }
        }
    }
}

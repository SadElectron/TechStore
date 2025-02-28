using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Results;
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
        public async Task<double> GetLastImageOrderAsync(Guid id)
        {
            using EfDbContext context = new EfDbContext();
            double? maxImageOrder = await context.Images
                .Where(i => i.ProductId == context.Images
                    .Where(i2 => i2.Id == id).Select(i2 => i2.ProductId).SingleOrDefault())
                .MaxAsync(e => (double?)e.ImageOrder);

            // Return the maximum ImageOrder or 0 if no images exist
            return maxImageOrder ?? 0;
        }
        public async Task<IEnumerable<Image>> AddAllAsync(ICollection<Image> images)
        {

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

                // Fetch the last ImageOrder for the specific image
                double lastImageOrder = await context.Images
                    .Where(i => i.ProductId == productId)
                    .OrderByDescending(i => i.ImageOrder)
                    .Select(i => i.ImageOrder)
                    .FirstOrDefaultAsync();

                var timeNowUtc = DateTimeHelper.GetUtcNow();

                foreach (var image in images)
                {
                    lastOrder += 1;
                    image.RowOrder = lastOrder;
                    if (image.ImageOrder == 0)
                    {
                        lastImageOrder++;
                        image.ImageOrder = lastImageOrder;
                    }
                    image.LastUpdate = timeNowUtc;
                    image.CreatedAt = timeNowUtc;
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
        public async Task<Image?> UpdateImageOrderAsync(Guid imageId, double newOrder)
        {
            using var context = new EfDbContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var timeNow = DateTimeHelper.GetUtcNow();
            try
            {
                var image = await context.Images.Where(i => i.Id == imageId)
                    .Select(i => new { i.Id, i.ImageOrder, i.ProductId })
                    .SingleOrDefaultAsync();
                if (image is null) return null;
                if (image.ImageOrder < newOrder)
                {
                    // Shift entities up
                    await context.Images.Where(i => i.ImageOrder > image.ImageOrder && i.ImageOrder <= newOrder && i.ProductId == image.ProductId)
                        .ExecuteUpdateAsync(s => s.SetProperty(i => i.ImageOrder, i => i.ImageOrder - 1));
                    await context.Images.Where(i => i.Id == imageId).ExecuteUpdateAsync(s => s
                        .SetProperty(i => i.ImageOrder, Math.Floor(newOrder))
                        .SetProperty(i => i.LastUpdate, timeNow));


                }
                else if (image.ImageOrder > newOrder)
                {
                    // Shift entities down
                    await context.Images.Where(i => i.ImageOrder < image.ImageOrder && i.ImageOrder >= newOrder && i.ProductId == image.ProductId)
                        .ExecuteUpdateAsync(s => s.SetProperty(i => i.ImageOrder, i => i.ImageOrder + 1));
                    await context.Images.Where(i => i.Id == imageId).ExecuteUpdateAsync(s => s
                        .SetProperty(i => i.ImageOrder, Math.Ceiling(newOrder))
                        .SetProperty(i => i.LastUpdate, timeNow));

                }

                await transaction.CommitAsync();
                return await context.Images.FindAsync(image.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public override async Task<EntityDeleteResult> DeleteAsync(Guid id)
        {
            using var context = new EfDbContext();
            var transaction = context.Database.BeginTransaction();
            var image = await context.Images.FindAsync(id);
            try
            {
                var rowOrder = image!.RowOrder;
                var imageOrder = image.ImageOrder;
                int result = await context.Images.Where(i => i.Id == image.Id).ExecuteDeleteAsync();
                if (result > 0)
                {
                    await context.Images.Where(i => i.RowOrder > rowOrder).ExecuteUpdateAsync(s => s.SetProperty(i => i.RowOrder, i => i.RowOrder - 1));
                    await context.Images.Where(i => i.ImageOrder > imageOrder && i.ProductId == image.ProductId).ExecuteUpdateAsync(s => s.SetProperty(i => i.ImageOrder, i => i.ImageOrder - 1));
                    await transaction.CommitAsync();
                    return new EntityDeleteResult(true, "Image deleted successfully");
                }
                else
                {
                    return new EntityDeleteResult(false, "Image not found");
                }


            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new EntityDeleteResult(false, $"Something Went Wrong {ex.Message}");
            }
        }
    }
}

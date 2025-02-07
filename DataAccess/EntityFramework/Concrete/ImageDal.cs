using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class ImageDal : EfDbRepository<Image, EfDbContext>, IImageDal
    {
        public ImageDal()
        {

        }
        public async Task<IEnumerable<Image>> BulkAddAsync(ICollection<Image> images)
        {
            using var context = new EfDbContext();
            int lastOrder = await context.Images.Where(i => i.ProductId == images.First().ProductId).OrderByDescending(i => i.Order).Select(i => i.Order).FirstOrDefaultAsync();
            foreach (var image in images)
            {
                lastOrder += 1;
                image.Order = lastOrder;
                image.LastUpdate = DateTime.UtcNow;
            }
            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();
            var addedImages = await context.Images
                .Where(i => i.ProductId == images.First().ProductId)
                .OrderBy(i => i.Order)
                .AsNoTracking()
                .ToListAsync();

            return addedImages;
        }
        public Task<int> DeleteImagesAsync(Guid productId)
        {
            using var context = new EfDbContext();
            return context.Images.Where(i => i.ProductId == productId).ExecuteDeleteAsync();

        }

        public async Task<IEnumerable<Image>> DeleteAsync(Guid imageId)
        {
            using var context = new EfDbContext();

            var image = await context.Images.SingleOrDefaultAsync(i => i.Id == imageId);
            Guid productId = image.ProductId;

            context.Images.Remove(image);
            await context.SaveChangesAsync();
            var currentImages = await context.Images.Where(i => i.ProductId == productId).ToListAsync();

            return currentImages;


        }
        public async Task<int> DeleteAndReorderAsync(Guid imageId)
        {
            using var context = new EfDbContext();
            var entity = await context.Images.Where(i => i.Id == imageId).AsNoTracking().SingleOrDefaultAsync();

            if (entity is null)
            {
                return 0;
            }

            int order = entity.Order;

            int deletedEntryCount = await context.Images.Where(i => i.Id == imageId).ExecuteDeleteAsync();

            if (deletedEntryCount > 0)
            {
                await context.Images.Where(i => i.Order > order).ExecuteUpdateAsync(s => s.SetProperty(i => i.Order, i => i.Order - 1));
            }

            return deletedEntryCount;
        }
        public async Task<Image?> UpdateOrderAsync(Guid imageId, int newOrder)
        {
            using var context = new EfDbContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var image = await context.Images.Where(i => i.Id == imageId).SingleOrDefaultAsync();
                if (image is null) return null;
                if (image.Order < newOrder)
                {
                    // Shift entities up
                    await context.Images.Where(i => i.Order > image.Order && i.Order <= newOrder && i.ProductId == image.ProductId)
                        .OrderBy(e => e.Order)
                        .ExecuteUpdateAsync(s => s.SetProperty(i => i.Order, i => i.Order - 1));


                }
                else if (image.Order > newOrder)
                {
                    // Shift entities down
                    await context.Images.Where(i => i.Order < image.Order && i.Order >= newOrder && i.ProductId == image.ProductId)
                        .OrderByDescending(e => e.Order)
                        .ExecuteUpdateAsync(s => s.SetProperty(i => i.Order, i => i.Order + 1));

                }
                image.LastUpdate = DateTime.UtcNow;
                image.Order = newOrder;
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

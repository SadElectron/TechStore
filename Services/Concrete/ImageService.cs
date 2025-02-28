using Core.Entities.Concrete;
using Core.Results;
using DataAccess.EntityFramework.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Abstract;
using System.Collections.Concurrent;

namespace Services.Concrete
{
    public class ImageService : IImageService
    {
        private readonly IImageDal _imageDal;
        private readonly IProductDal _productDal;
        private readonly ILogger<ImageService> _logger;
        private static readonly string[] AllowedFileTypes = { ".jpg", ".jpeg", ".png", ".webp" };
        public ImageService(IImageDal imageDal, IProductDal productDal, ILogger<ImageService> logger)
        {
            _imageDal = imageDal;
            _productDal = productDal;
            _logger = logger;
        }
        public Task<Image?> GetAsNoTrackingAsync(Guid id)
        {
            return _imageDal.GetAsNoTrackingAsync(i => i.Id == id);
        }
        public Task<List<Image>> GetImagesAsync(Guid productId)
        {
            return _imageDal.GetAllAsync(i => i.ProductId == productId, i => i.RowOrder);
        }
        public Task<List<Image>> GetAllAsNoTrackingAsync(Guid productId)
        {
            return _imageDal.GetAllAsNoTrackingAsync(i => i.ProductId == productId, i => i.RowOrder);
        }
        public Task<double> GetLastImageOrderAsync(Guid id)
        {
            return  _imageDal.GetLastImageOrderAsync(id);
        }
        public async Task<IEnumerable<Image>> BulkAddAsync(ICollection<IFormFile> formFiles, Guid productId)
        {

            var images = new ConcurrentBag<Image>();
            await Parallel.ForEachAsync(formFiles, async (file, cancellationToken) =>
            {

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream, cancellationToken);

                images.Add(new Image
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    File = memoryStream.ToArray()
                });

            });

            return await _imageDal.AddAllAsync(images.ToList());

        }
        public Task<Image?> UpdateOrderAsync(Guid imageId, double newOrder)
        {
            return _imageDal.UpdateImageOrderAsync(imageId, newOrder);
        }
        public async Task<int> DeleteImagesAsync(Guid productId)
        {

            bool exists = await _imageDal.ExistsAsync(i => i.ProductId == productId);
            if (!exists) return 0;

            return await _imageDal.DeleteImagesAsync(productId);
        }
        public async Task<EntityDeleteResult> DeleteAsync(Guid id)
        {
            return await _imageDal.DeleteAsync(id);
        }
        public Task<bool> ExistsAsync(Guid id)
        {
            return _imageDal.ExistsAsync(id);
        }
    }
}

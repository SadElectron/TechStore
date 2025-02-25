﻿using Core.Entities.Concrete;
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

        public async Task<IEnumerable<Image>> BulkAddAsync(ICollection<IFormFile> formFiles, Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Invalid product ID.");
            }
            if (formFiles == null || !formFiles.Any())
            {
                throw new ArgumentException("No files provided.", nameof(formFiles));
            }
            var productExists = await _productDal.ExistsAsync(productId);
            if (!productExists)
            {
                throw new ArgumentException($"Product with ID {productId} does not exist.");
            }
            var images = new ConcurrentBag<Image>();

            await Parallel.ForEachAsync(formFiles, async (file, cancellationToken) =>
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("Invalid file provided.");
                }

                if (!AllowedFileTypes.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    throw new ArgumentException($"File '{file.FileName}' is not an allowed file type.");
                }

                const int maxFileSize = 10 * 1024 * 1024; // 10MB
                if (file.Length > maxFileSize)
                {
                    throw new ArgumentException($"File '{file.FileName}' exceeds the maximum allowed size of 10MB.");
                }

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



        public Task<List<Image>> GetImagesAsync(Guid productId)
        {
            return _imageDal.GetAllAsync(i => i.ProductId == productId, i => i.RowOrder);
        }

        public Task<Image?> UpdateOrderAsync(Guid imageId, double newOrder)
        {
            return _imageDal.UpdateImageOrderAsync(imageId, newOrder);
        }

        public Task<List<Image>> GetAllAsNoTrackingAsync(Guid productId)
        {
            return _imageDal.GetAllAsNoTrackingAsync(i => i.ProductId == productId, i => i.RowOrder);
        }

        public async Task<EntityDeleteResult> DeleteAsync(Guid id)
        {
            try
            {
                return await _imageDal.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error in ImageService.DeleteAsync {ex.Message}");
                throw;
            }
        }
        public async Task<int> DeleteImagesAsync(Guid productId)
        {

            bool exists = await _imageDal.ExistsAsync(i => i.ProductId == productId);
            if (!exists) return 0;

            return await _imageDal.DeleteImagesAsync(productId);
        }

        public async Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id)
        {
            var entity = await _imageDal.ExistsAsync(id);
            if (!entity) return new EntityDeleteResult(false, "Entity not found");
            var i = await _imageDal.DeleteAndReorderAsync(id);
            return i > 0 ? new EntityDeleteResult(true, "Entity deleted") : new EntityDeleteResult(false, "Entity not deleted");
        }
    }
}

using Core.Dtos;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class ImageService : IImageService
    {
        private readonly IImageDal _imageDal;

        public ImageService(IImageDal imageDal)
        {
            _imageDal = imageDal;
        }

        public async Task<IEnumerable<Image>> BulkAddAsync(ICollection<IFormFile> formFiles, Guid productId)
        {

            List<Image> images = new List<Image>();
            foreach (var file in formFiles)
            {
                
                using MemoryStream memoryStream = new();
                await file.CopyToAsync(memoryStream);
                images.Add(new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    File = memoryStream.ToArray()
                });
            }
            return await _imageDal.AddAllAsync(images);
        }

        public Task<int> DeleteImagesAsync(Guid productId)
        {
            return _imageDal.DeleteImagesAsync(productId);
        }
        
        public Task<List<Image>> GetImagesAsync(Guid productId)
        {
            return _imageDal.GetAllAsync(i => i.ProductId == productId, i => i.RowOrder);
        }

        public Task<Image?> UpdateOrderAsync(Guid imageId, int newOrder)
        {
            return _imageDal.UpdateOrderAsync(imageId, newOrder);
        }

        public Task<List<Image>> GetAllAsNoTrackingAsync(Guid productId)
        {
            return _imageDal.GetAllAsNoTrackingAsync(i => i.ProductId == productId, i => i.RowOrder);
        }

        public async Task<Image> DeleteAsync(Guid imageId)
        {
            var image = await _imageDal.GetAsync(i => i.Id == imageId);
            return await _imageDal.DeleteAsync(image);
        }
        
        public async Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id)
        {
            var entity = await _imageDal.GetAsync(c => c.Id == id);
            if (entity == null) return new EntityDeleteResult(false, "Entity not found");
            var i = await _imageDal.DeleteAndReorderAsync(id);
            return i > 0 ? new EntityDeleteResult(true, "Entity deleted") : new EntityDeleteResult(false, "Entity not deleted");
        }
    }
}

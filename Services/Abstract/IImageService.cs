using Core.Entities.Concrete;
using Core.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesAsync(Guid productId);
        Task<List<Image>> GetAllAsNoTrackingAsync(Guid productId);
        Task<IEnumerable<Image>> BulkAddAsync(ICollection<IFormFile> images, Guid productId);
        Task<Image?> UpdateOrderAsync(Guid imageId, double newOrder);
        Task<Image> DeleteAsync(Guid imageId);
        Task<int> DeleteImagesAsync(Guid productId);
        Task<EntityDeleteResult> DeleteAndReorderAsync(Guid id);
    }
}

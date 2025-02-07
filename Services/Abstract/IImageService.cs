using Core.Entities.Concrete;
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
        Task<int> DeleteAndReorderAsync(Guid imageId);
        Task<IEnumerable<Image>> DeleteAsync(Guid imageId);
        Task<Image?> UpdateOrderAsync(Guid imageId, int newOrder);
        Task<int> DeleteImagesAsync(Guid productId);
    }
}

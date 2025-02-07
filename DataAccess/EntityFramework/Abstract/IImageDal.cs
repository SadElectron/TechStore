using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IImageDal : IEfDbRepository<Image>
    {
        Task<IEnumerable<Image>> BulkAddAsync(ICollection<Image> images);
        Task<int> DeleteImagesAsync(Guid productId);
        Task<int> DeleteAndReorderAsync(Guid imageId);
        Task<IEnumerable<Image>> DeleteAsync(Guid imageId);
        Task<Image?> UpdateOrderAsync(Guid imageId, int newOrder);
    }
}

using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IImageDal : IEfDbRepository<Image>
    {
        Task<IEnumerable<Image>> AddAllAsync(ICollection<Image> images);
        Task<int> DeleteImagesAsync(Guid productId);
        Task<Image?> UpdateOrderAsync(Guid imageId, double newOrder);
        Task<double> GetLastImageOrderAsync(Expression<Func<Image, bool>> filter);
        new Task<int> DeleteAndReorderAsync(Guid id);
    }
}

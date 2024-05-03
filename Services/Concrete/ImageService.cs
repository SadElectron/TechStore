using DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Task AddAsync(Image entity)
        {
            return _imageDal.AddAsync(entity);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var image = await _imageDal.GetAsync<Image>(i => i.ImageId == id);

            return await _imageDal.DeleteAsync(image);
        }

        public Task<List<Image>> GetAllAsync(Expression<Func<Image, bool>>? filter)
        {
            return _imageDal.GetAllAsync(filter);
        }

        public Task<Image> GetAsync(Expression<Func<Image, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetRecordCountAsync<Image>()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Image entity)
        {
            throw new NotImplementedException();
        }
    }
}

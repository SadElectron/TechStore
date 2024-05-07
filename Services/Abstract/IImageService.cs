using Entities.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IImageService
    {
        public Task<List<Image>> GetAllAsync(Expression<Func<Image, bool>>? filter);
        public Task<Image?> GetAsync(Guid id);
        public Task AddAsync(Image entity);
        public Task<List<Image>> BulkAddAsync(List<Image> Images);
        public Task UpdateAsync(Image entity);
        public Task<int> DeleteAsync(Guid id);
        public Task<int> GetRecordCountAsync<Image>();
    }
}

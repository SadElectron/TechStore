using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface ICPUService
    {
        public Task<List<CPU>> GetAllAsync();
        public Task<CPU> GetByIdAsync(Guid id);
        public Task AddAsync(CPU entity);
        public Task UpdateAsync(CPU entity);
        public Task DeleteAsync(CPU entity);
        public Task<int> GetRecordCountAsync();
        public Task<CPU> GetWithImagesAsync(Guid id);
    }
}

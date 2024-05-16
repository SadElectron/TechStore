using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IGPUService
    {
        public Task<List<GPU>> GetAllAsync();
        public Task<GPU?> GetByIdAsync(Guid? id);
        public Task<GPU> AddAsync(GPU entity);
        public Task UpdateAsync(GPU entity);
        public Task DeleteAsync(GPU entity);
        public Task<int> GetRecordCountAsync();
    }
}

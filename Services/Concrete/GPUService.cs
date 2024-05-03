using DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class GPUService : IGPUService
    {
        private readonly IGPUDal _gpudal;
        public GPUService(IGPUDal gpudal)
        {
            _gpudal = gpudal;
        }
        public Task AddAsync(GPU entity)
        {
            return _gpudal.AddAsync(entity);
        }

        public Task DeleteAsync(GPU entity)
        {
            return _gpudal.DeleteAsync(entity);
        }

        public Task<GPU> GetByIdAsync(Guid? id)
        {
            return _gpudal.GetAsync(g => g.Id == id);
        }

        public Task<List<GPU>> GetAllAsync()
        {
            return _gpudal.GetAllAsync<GPU>(null);
        }

        public Task UpdateAsync(GPU entity)
        {
            return _gpudal.UpdateAsync(entity);
        }

        public Task<int> GetRecordCountAsync()
        {
            return _gpudal.GetRecordCountAsync();
        }
    }
}

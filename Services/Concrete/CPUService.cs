using DataAccess.EntityFramework.Abstract;
using DataAccess.EntityFramework.Concrete;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class CPUService : ICPUService
    {
        private readonly ICPUDal _cpudal;
        public CPUService(ICPUDal cpudal)
        {
            _cpudal = cpudal;
        }
        public Task<CPU> AddAsync(CPU entity)
        {
            return _cpudal.AddAsync(entity);
        }

        public Task DeleteAsync(CPU entity)
        {
            return _cpudal.DeleteAsync(entity);
        }

        public Task<CPU> GetByIdAsync(Guid id)
        {
            return _cpudal.GetAsync(c => c.Id == id);
        }

        public Task<List<CPU>> GetAllAsync()
        {
            return _cpudal.GetAllAsync<CPU>(null);
        }

        public Task UpdateAsync(CPU entity)
        {
            return _cpudal.UpdateAsync(entity);
        }

        public Task<int> GetRecordCountAsync()
        {
            return _cpudal.GetRecordCountAsync();
        }

    }
}

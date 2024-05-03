using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Abstract
{
    public interface IEfDbContextBase<T> where T : class, IEntity
    {
        public Task<List<U>> GetAllAsync<U>(Expression<Func<U, bool>>? filter) where U : class, IEntity;
        public Task<U> GetAsync<U>(Expression<Func<U, bool>> filter) where U : class, IEntity;
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task<int> DeleteAsync(T entity);
        public Task<int> GetRecordCountAsync<U>() where U : class, IEntity;
    }


}

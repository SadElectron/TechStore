using Entities.Abstract;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Abstract
{
    public interface IEfDbContextBase 
    {
        Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>>? filter) where T : class, IEntity;
        Task<T?> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity;
        Task<T> AddAsync<T>(T entity) where T : class, IEntity;
        Task UpdateAsync<T>(T entity) where T : class, IEntity;
        Task<int> DeleteAsync<T>(T entity) where T : class, IEntity;
        Task<int> GetRecordCountAsync<T>() where T : class, IEntity;
    }


}

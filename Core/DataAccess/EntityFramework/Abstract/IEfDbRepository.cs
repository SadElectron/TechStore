using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Abstract
{
    public interface IEfDbRepository<TEntity>
    {
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(int page, int itemCount, Expression<Func<TEntity, object>> orderFilter);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, object>> orderFilter);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, object>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync(int page, int itemCount, Expression<Func<TEntity, object>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, object>> orderFilter);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task UpdateAllAsync(List<TEntity> entities);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<int> GetEntryCountAsync();
        Task<int> GetEntryCountAsync(Expression<Func<TEntity, bool>> filter);
        Task SaveChangesAsync();
        
        
    }


}

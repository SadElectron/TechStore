using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
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
        Task<List<TEntity>> GetAllAsync<Tkey>(int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter);
        Task<List<TEntity>> GetAllAsync<Tkey>(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter);
        Task<List<TEntity>> GetAllAsync<Tkey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, Tkey>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(Expression<Func<TEntity, Tkey>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, Tkey>> orderFilter);
        Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter);
        Task<int> GetEntryCountAsync();
        Task<int> GetEntryCountAsync(Expression<Func<TEntity, bool>> filter);
        Task<double> GetLastOrderAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task UpdateAllAsync(List<TEntity> entities);
        Task<TEntity> UpdateAndReorderAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<int> DeleteAndReorderAsync(Guid id);
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(Guid id);
        
    }


}

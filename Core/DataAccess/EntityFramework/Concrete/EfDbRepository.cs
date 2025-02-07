using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Concrete
{
    public class EfDbRepository<TEntity, TContext> : IEfDbRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext, new()
    {
        public Task<List<TEntity>> GetAllAsync(int page, int itemCount, Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().Where(filter).OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().Where(filter).OrderBy(orderFilter).ToListAsync();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            using var context = new TContext();
            return context.Set<TEntity>().ToListAsync();
        }

        public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {

                return context.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
        }
        public Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                return addedEntity.Entity;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using (var context = new TContext())
            {

                var updatedEntity = context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();
                return updatedEntity.Entity;
            }
        }
        public async Task UpdateAllAsync(List<TEntity> entities)
        {
            using (var context = new TContext())
            {

                context.Set<TEntity>().UpdateRange(entities);
                await context.SaveChangesAsync();

            }
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            using var context = new TContext();
            var deletedEntity = context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
            return deletedEntity.Entity;

        }
        public async Task<int> GetEntryCountAsync()
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().CountAsync();
            }
        }

        public Task<int> GetEntryCountAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Where(filter).CountAsync();
            }
        }

        public Task SaveChangesAsync()
        {
            using var context = new TContext();
            return context.SaveChangesAsync();
        }
        public Task<List<TEntity>> GetAllAsNoTrackingAsync(int page, int itemCount, Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).AsNoTracking().ToListAsync();

        }
        public Task<List<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().Where(filter).OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).AsNoTracking().ToListAsync();
        }
        public Task<List<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().Where(filter).OrderBy(orderFilter).AsNoTracking().ToListAsync();
        }

        public Task<List<TEntity>> GetAllAsNoTrackingAsync(Expression<Func<TEntity, object>> orderFilter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().OrderBy(orderFilter).AsNoTracking().ToListAsync();
        }
    }
}

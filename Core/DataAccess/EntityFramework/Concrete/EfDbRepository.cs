using Core.DataAccess.EntityFramework.Abstract;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Concrete
{
    public abstract class EfDbRepository<TEntity, TContext> : IEfDbRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext, new()
    {
        private readonly ILogger<TEntity> _logger;
        public EfDbRepository(ILogger<TEntity> logger)
        {
            _logger = logger;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(filter);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            using var context = new TContext();
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync<Tkey>(int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync<Tkey>(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().Where(filter).OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync<Tkey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().Where(filter).OrderBy(orderFilter).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().OrderBy(orderFilter).AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().Where(filter).OrderBy(orderFilter).AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsNoTrackingAsync<Tkey>(Expression<Func<TEntity, bool>> filter, int page, int itemCount, Expression<Func<TEntity, Tkey>> orderFilter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().Where(filter).OrderBy(orderFilter).Skip((page - 1) * itemCount).Take(itemCount).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using var context = new TContext();
            var addedEntity = await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using var context = new TContext();
            var updatedEntity = context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return updatedEntity.Entity;
        }

        public async Task UpdateAllAsync(List<TEntity> entities)
        {
            using var context = new TContext();
            context.Set<TEntity>().UpdateRange(entities);
            await context.SaveChangesAsync();
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
            using var context = new TContext();
            return await context.Set<TEntity>().CountAsync();
        }

        public async Task<int> GetEntryCountAsync(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().Where(filter).CountAsync();
        }
        public async Task<double> GetLastOrderAsync()
        {
            using var context = new TContext();
            var lastOrder = await context.Set<TEntity>()
                .OrderByDescending(u => u.RowOrder)
                .Select(u => u.RowOrder)
                .FirstOrDefaultAsync();
            return lastOrder;
        }

        public async Task SaveChangesAsync()
        {
            using var context = new TContext();
            await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAndReorderAsync(Guid id)
        {
            using var context = new TContext();
            var db = context.Set<TEntity>();

            double rowOrder = await db.Where(e => e.Id == id)
                                 .Select(e => e.RowOrder)
                                 .SingleOrDefaultAsync();
            int deletedEntryCount = await db.Where(p => p.Id == id).ExecuteDeleteAsync();
            if (deletedEntryCount > 0)
            {
                await db.Where(c => c.RowOrder > rowOrder)
                        .ExecuteUpdateAsync(s => s.SetProperty(c => c.RowOrder, p => p.RowOrder - 1));
            }

            return deletedEntryCount;
        }
        public async Task<TEntity> UpdateAndReorderAsync(TEntity entity)
        {
            using var context = new TContext();
            var db = context.Set<TEntity>();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                double oldRowOrder = await db.Where(e => e.Id == entity.Id).Select(e => e.RowOrder).SingleOrDefaultAsync();
                if (oldRowOrder < entity.RowOrder)
                {
                    // Shift entities down
                    await db.Where(c => c.RowOrder > oldRowOrder && c.RowOrder <= entity.RowOrder)
                        .ExecuteUpdateAsync(c => c
                            .SetProperty(c => c.RowOrder, c => c.RowOrder - 1)
                            .SetProperty(c => c.LastUpdate, DateTimeHelper.GetUtcNow()));
                }
                else if (oldRowOrder > entity.RowOrder)
                {
                    // Shift entities up
                    await db.Where(c => c.RowOrder < oldRowOrder && c.RowOrder >= entity.RowOrder)
                        .ExecuteUpdateAsync(c => c
                            .SetProperty(c => c.RowOrder, c => c.RowOrder + 1)
                            .SetProperty(c => c.LastUpdate, DateTimeHelper.GetUtcNow()));
                }
                entity.LastUpdate = DateTimeHelper.GetUtcNow();
                db.Update(entity);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error in CategoryDal.UpdateAndReorderAsync {e.Message}");
                throw;
            }
        }
    }
}

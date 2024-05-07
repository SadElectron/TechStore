using Core.DataAccess.EntityFramework.Abstract;
using Entities.Abstract;
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
    public class EfEntityContextBase<TEntity, TContext> : IEfDbContextBase<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public Task<List<U>> GetAllAsync<U>(Expression<Func<U, bool>>? filter) where U : class, IEntity
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<U>().ToListAsync()
                    : context.Set<U>().Where(filter).ToListAsync();
            }
        }

        public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefaultAsync(filter);
            }
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

        public Task UpdateAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                return context.SaveChangesAsync();
            }
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deleteEntity = context.Entry(entity);
                deleteEntity.State = EntityState.Deleted;
                return context.SaveChangesAsync();
            }
        }

        public Task<int> GetRecordCountAsync<U>() where U : class, IEntity
        {
            using (var context = new TContext())
            {
                return context.Set<U>().CountAsync();
            }
        }
    }
}

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

        public Task<U> GetAsync<U>(Expression<Func<U, bool>> filter) where U : class, IEntity
        {
            using (var context = new TContext())
            {
                return context.Set<U>().SingleOrDefaultAsync(filter);
            }
        }

        public Task AddAsync(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                return context.SaveChangesAsync();
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

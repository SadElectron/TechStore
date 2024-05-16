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
    public class EfEntityContextBase<TContext> : IEfDbContextBase
        where TContext : DbContext, new()
    {
        public Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>>? filter) where T : class, IEntity
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<T>().ToListAsync()
                    : context.Set<T>().Where(filter).ToListAsync();
            }
        }

        public Task<T?> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class, IEntity
        {
            using (var context = new TContext())
            {
                return context.Set<T>().SingleOrDefaultAsync(filter);
            }
        }

        public async Task<T> AddAsync<T>(T entity) where T : class, IEntity
        {
            using (var context = new TContext())
            {
                var addedEntity = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return addedEntity.Entity;
            }
        }

        public Task UpdateAsync<T>(T entity) where T : class, IEntity
        {
            using (var context = new TContext())
            {
                
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                return context.SaveChangesAsync();
            }
        }

        public Task<int> DeleteAsync<T>(T entity) where T : class, IEntity
        {
            using (var context = new TContext())
            {
                var deleteEntity = context.Entry(entity);
                deleteEntity.State = EntityState.Deleted;
                return context.SaveChangesAsync();
            }
        }

        public Task<int> GetRecordCountAsync<T>() where T : class, IEntity
        {
            using (var context = new TContext())
            {
                return context.Set<T>().CountAsync();
            }
        }
    }
}

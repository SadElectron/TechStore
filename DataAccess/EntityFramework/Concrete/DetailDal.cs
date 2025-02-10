using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class DetailDal : EfDbRepository<Detail, EfDbContext>, IDetailDal
    {
        public async Task<Detail> AddOrderedAsync(Detail detail)
        {
            using EfDbContext context = new EfDbContext();

            double order = await context.Details.OrderByDescending(d => d.RowOrder).Take(3).Select(d => d.RowOrder).FirstOrDefaultAsync();

            detail.RowOrder = order + 1;

            var addedEntity = await context.Details.AddAsync(detail);
            await context.SaveChangesAsync();

            return addedEntity.Entity;

        }

        public async Task<List<Detail>> GetAllWithPropsAsNoTrackingAsync(Guid productId)
        {
            using EfDbContext context = new EfDbContext();
            return await context.Details.Where(d => d.ProductId == productId)
                .Include(d => d.Property)
                .OrderBy(d => d.Property!.PropOrder)
                .ToListAsync();
        }
        public async Task<List<Detail>> UpdateDetailsAsync(List<Detail> details)
        {
            using var context = new EfDbContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.Details.UpdateRange(details);
                await context.SaveChangesAsync();
                return details;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}

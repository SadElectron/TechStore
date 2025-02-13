using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<Detail> _logger;

        public DetailDal(ILogger<Detail> logger):base(logger)
        {
            _logger = logger;
        }

        public Task<double> GetLastOrderAsync()
        {
            using EfDbContext context = new EfDbContext();
            var lastOrder = context.Details.OrderByDescending(e => e.RowOrder).Select(e => e.RowOrder).FirstOrDefaultAsync();
            return lastOrder;
        }

        public async Task<List<Detail>> GetAllWithPropsAsync(Guid productId)
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
                await transaction.CommitAsync();
                return details;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError($"Error in DetailDal.UpdateDetailsAsync {e.Message}");
                throw;
            }
        }

    }
}

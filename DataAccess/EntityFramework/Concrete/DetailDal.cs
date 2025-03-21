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

        public DetailDal(ILogger<Detail> logger) : base(logger)
        {
            _logger = logger;
        }
        public async Task<List<Detail>> GetByIdsAsync(List<Guid> ids)
        {
            using EfDbContext context = new EfDbContext();
            return await context.Details.Where(d => ids.Contains(d.Id))
                .ToListAsync();
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
                return new List<Detail>();
            }
        }
        public async Task<int> DeleteRangeAsync(ICollection<Detail> details)
        {
            if (details == null || !details.Any()) return 0;

            using var context = new EfDbContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {

                var firstRowOrder = details.Min(d => d.RowOrder);

                var ids = details.Select(d => d.Id).ToList();

                int deletedEntryCount = await context.Details.Where(d => ids.Contains(d.Id)).ExecuteDeleteAsync();
                if (deletedEntryCount == ids.Count)
                {

                    var detailsGreater = await context.Details.Where(c => c.RowOrder >= firstRowOrder).ToListAsync();
                    double newRowOrder = firstRowOrder;
                    foreach (var detail in detailsGreater)
                    {
                        detail.RowOrder = newRowOrder;
                        newRowOrder++;
                    }
                    context.Details.UpdateRange(detailsGreater);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return deletedEntryCount;

                }
                await transaction.RollbackAsync();
                return 0;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Error in DetailDal.DeleteRangeAsync {ex.Message}");
                return 0;
            }

        }

    }
}

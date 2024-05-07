using Core.DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IImageDal : IEfDbContextBase<Image>
    {
        Task<List<Image>> BulkAddAsync(List<Image> Images);
    }
}

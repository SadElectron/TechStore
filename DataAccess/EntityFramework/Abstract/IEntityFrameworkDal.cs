using Core.DataAccess.EntityFramework.Abstract;
using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Abstract
{
    public interface IEntityFrameworkDal 
    {
        Task<int> GetRecordCountAsync<T>() where T : class;

    }
}

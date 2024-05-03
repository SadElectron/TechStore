using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IEntityFrameworkService
    {

        public Task<int> GetRecordCountAsync<T>() where T : class, IEntity;
    }
}

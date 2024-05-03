using DataAccess.EntityFramework.Abstract;
using Entities.Abstract;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class EntityFrameworkService : IEntityFrameworkService
    {
        private readonly IEntityFrameworkDal _entityFrameworkDal;

        public EntityFrameworkService(IEntityFrameworkDal entityFrameworkDal)
        {
            _entityFrameworkDal = entityFrameworkDal;
        }

        public Task<int> GetRecordCountAsync<T>() where T : class, IEntity
        {
            return _entityFrameworkDal.GetRecordCountAsync<T>();
        }
    }
}

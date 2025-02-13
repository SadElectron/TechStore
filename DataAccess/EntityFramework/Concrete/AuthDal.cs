using Core.DataAccess.EntityFramework.Concrete;
using Core.Entities.Concrete;
using DataAccess.EntityFramework.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class AuthDal : EfDbRepository<User, EfDbContext>, IAuthDal
    {
        private readonly ILogger<User> _logger;
        public AuthDal(ILogger<User> logger) : base(logger)
        {
            _logger = logger;
        }
    }
}

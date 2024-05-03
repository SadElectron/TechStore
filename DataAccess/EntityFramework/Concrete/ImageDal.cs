using Core.DataAccess.EntityFramework.Concrete;
using DataAccess.EntityFramework.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Concrete
{
    public class ImageDal :EfEntityContextBase<Image, EfDbContext>, IImageDal
    {

    }
}

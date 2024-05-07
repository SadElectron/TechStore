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
    public class ImageDal : EfEntityContextBase<Image, EfDbContext>, IImageDal
    {
        public async Task<List<Image>> BulkAddAsync(List<Image> Images)
        {
            using var context = new EfDbContext();
            var addedImages = new List<Image>();
            foreach (var image in Images)
            {
                var addedImageEntity = await context.Images.AddAsync(image);
                addedImages.Add(addedImageEntity.Entity);
            }
            await context.SaveChangesAsync();
            return addedImages;
        }
    }
}

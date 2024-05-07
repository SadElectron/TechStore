using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Thumbnail
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public required Image? Image { get; set; }
        public required byte[] ThumbnailImage { get; set; }
    }
}

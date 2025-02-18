using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double ImageOrder { get; set; }
        public required byte[] File { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

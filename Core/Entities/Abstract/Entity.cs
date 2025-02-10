using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Abstract
{
    public abstract class Entity
    {

        public Guid Id { get; set; }
        public double RowOrder { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

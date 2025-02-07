using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public required string CategoryName { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}

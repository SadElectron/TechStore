using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Product : Entity
    {
        public Guid CategoryId { get; set; }
        public required string ProductName { get; set; }
        public int Stock { get; set; }
        public int SoldQuantity { get; set; }
        public double Price { get; set; }
        public Category? Category { get; set; }
        public ICollection<Detail> Details { get; set; } = new List<Detail>();
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}

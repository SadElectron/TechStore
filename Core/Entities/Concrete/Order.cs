using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double TotalPrice { get; set; }
        public required string Adress { get; set; }
        public int RowOrder { get; set; }
        public required List<OrderProduct> OrderProducts { get; set; }
    }
}

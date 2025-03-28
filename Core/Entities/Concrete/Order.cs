﻿using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Order: Entity, IEntity
    {
        public Guid UserId { get; set; }
        public double TotalPrice { get; set; }
        public required string Adress { get; set; }
        public required ICollection<OrderProduct> ProductOrders { get; set; }
    }
}

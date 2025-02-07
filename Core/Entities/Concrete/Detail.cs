using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Detail : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid PropertyId { get; set; }
        public int Order { get; set; }
        public required string PropValue { get; set; }
        public DateTime LastUpdate { get; set; }
        public Property? Property { get; set; }
        public Product? Product { get; set; }

    }
}

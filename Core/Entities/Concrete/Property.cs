using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Property : IEntity
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public int Order { get; set; }
        public required string PropName { get; set; }
        public DateTime LastUpdate { get; set; }
        
        public Category? Category { get; set; }
        public ICollection<Detail> Details { get; set; } = new List<Detail>();
    }
}

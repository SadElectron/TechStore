using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Detail : Entity
    {
        public required string PropValue { get; set; }


        public Guid PropertyId { get; set; }
        public Property? Property { get; set; }

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

    }
}

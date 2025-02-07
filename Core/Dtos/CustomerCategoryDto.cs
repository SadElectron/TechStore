using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class CustomerCategoryDto
    {
        public Guid Id { get; set; }
        public required string CategoryName { get; set; }
        public ICollection<CustomerProductDto> Products { get; set; } = new List<CustomerProductDto>();
    }
}

using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Dtos;

public class CustomerProductDto
{
    public Guid Id { get; set; }
    public required string ProductName { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    public ICollection<CustomerDetailDto> Details { get; set; } = new List<CustomerDetailDto>();
    public ICollection<CustomerImageDto> Images { get; set; } = new List<CustomerImageDto>();
}

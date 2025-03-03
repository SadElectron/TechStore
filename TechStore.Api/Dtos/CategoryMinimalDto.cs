using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Dtos;

public class CategoryMinimalDto
{
    public Guid Id { get; set; }
    public double RowOrder { get; set; }
    public required string CategoryName { get; set; }
    public DateTime LastUpdate { get; set; }
}

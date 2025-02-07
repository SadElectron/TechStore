using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public DateTime LastUpdate { get; set; }
}
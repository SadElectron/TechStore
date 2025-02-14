using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos;

public class DetailMinimalDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid ProductId { get; set; }
    public required string PropValue { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime CreatedAt { get; set; }
   
    
}

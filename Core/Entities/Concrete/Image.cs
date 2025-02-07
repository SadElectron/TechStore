using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete;

public class Image : IEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Order { get; set; }
    public required byte[] File { get; set; }
    public DateTime LastUpdate { get; set; }
    public Product? Product { get; set; }
}


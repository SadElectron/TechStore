using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete;

public class Image : Entity
{
    public Guid ProductId { get; set; }
    public required byte[] File { get; set; }
    public Product? Product { get; set; }
}


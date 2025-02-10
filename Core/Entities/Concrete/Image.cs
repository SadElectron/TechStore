using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete;

public class Image : Entity, IEntity
{
    public double ImageOrder { get; set; }
    public required byte[] File { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}


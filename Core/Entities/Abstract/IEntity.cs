using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Abstract;

public interface IEntity
{
    public Guid Id { get; set; }
    public double RowOrder { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime CreatedAt { get; set; }

}

using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class UserData: Entity, IEntity
    {
        public Guid UserId { get; set; }
        public required string DataName { get; set; }
        public required string DataValue { get; set; }
        public User? User { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class UserData
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string DataName { get; set; }
        public required string DataValue { get; set; }
        public int RowOrder { get; set; }
        public User? User { get; set; }

    }
}

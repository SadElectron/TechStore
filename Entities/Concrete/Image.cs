﻿using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Image : IEntity
    {
        public Guid ImageId { get; set; }
        public Guid ProductId { get; set; }
        public bool Type { get; set; }
        public required byte[] File{ get; set; }
    }
}

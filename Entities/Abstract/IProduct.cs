﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Abstract
{
    public interface IProduct
    {
        Guid Id { get; set; }
        string Brand { get; set; }
        string ModelName { get; set; }

    }
}

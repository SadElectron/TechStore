﻿using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class RefreshToken : IEntity
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}

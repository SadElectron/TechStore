﻿using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos;

public record RegisterUserResult(User user, bool success, string failReason = "");
public record LoginResult(Guid Id, string Email, string Token, bool Status);




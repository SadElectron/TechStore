using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Authorization.Requirements;

public class RoleRequirement(string role) : IAuthorizationRequirement
{
    public string Role { get; } = role;
}

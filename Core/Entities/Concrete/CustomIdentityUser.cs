using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete;

public class CustomIdentityUser : IdentityUser
{
    public string RefreshToken { get; set; }  = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; } 
    public DateTime Created { get; set; }
}

using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract;

public interface ITokenService
{
    string Create(CustomIdentityUser user, IList<string> roles);
    string CreateRefreshToken(CustomIdentityUser user);
}

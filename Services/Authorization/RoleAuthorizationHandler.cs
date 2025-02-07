using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Services.Authorization.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Authorization
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IConfiguration _configuration;
        public RoleAuthorizationHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            var roleClaim = context.User.Claims.Where(c => c.Type == "Role").SingleOrDefault();

            if (roleClaim is null)
            {
                return Task.CompletedTask;
            }

            if (roleClaim.Value == requirement.Role)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            
            return Task.CompletedTask;
        }
    }
}

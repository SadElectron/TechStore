using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Authorization.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Authorization;

public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RoleAuthorizationHandler> _logger;
    private const string RoleClaimType = "Role";
    public RoleAuthorizationHandler(IConfiguration configuration, ILogger<RoleAuthorizationHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        try
        {
            var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == RoleClaimType);

            if (roleClaim == null)
            {
                return Task.CompletedTask;
            }

            if (roleClaim.Value == requirement.Role)
            {
                context.Succeed(requirement);
                //_logger.LogInformation($"Authorization succeeded for role: {requirement.Role}");
            }
            else
            {
                context.Fail();
                //_logger.LogWarning($"Authorization failed. User role: {roleClaim.Value}, required role: {requirement.Role}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while handling the role authorization. {ex.Message}");
            context.Fail();
        }
        return Task.CompletedTask;
    }
}

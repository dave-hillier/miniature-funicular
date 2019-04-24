using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ScopeClaim
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (context.User.Claims.Any(claim => claim.Type == "scope" &&
              claim.Value.Contains(requirement.Scope, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

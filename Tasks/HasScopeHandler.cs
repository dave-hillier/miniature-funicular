using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tasks
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (context.User.Claims.Any(claim => claim.Type == "scope" &&
              claim.Issuer == requirement.Issuer &&
              claim.Value.Contains(requirement.Scope)))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

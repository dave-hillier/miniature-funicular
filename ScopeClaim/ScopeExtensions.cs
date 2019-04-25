using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ScopeClaim
{
    public static class ScopeExtensions
    {
        public static void RequireScope(this AuthorizationPolicyBuilder policy, string scope)
        {
            policy.Requirements.Add(new HasScopeRequirement(scope));
        }
        
        public static void AddScopePolicies(this AuthorizationOptions options, params string[] scopes)
        {
            foreach (var scope in scopes)
            {
                options.AddPolicy(scope, policy => policy.RequireScope(scope));
            }
        }

    }
}
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Users
{
    public interface ITenantAccessor
    {
        string CurrentTenant { get; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class TenantAccessor : ITenantAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public TenantAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string CurrentTenant
        {
            get
            {
                var accessorHttpContext = _accessor.HttpContext;
                return accessorHttpContext.User.Claims.Single(c => c.Type == "https://auth.guestline.app/claims/tenant")
                    .Value;
            }
        }
    }
}
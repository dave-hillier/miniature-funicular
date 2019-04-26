using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Issues
{
    public interface ITenantAccessor
    {
        string CurrentTenant { get; }
    }

    class TenantAccessor : ITenantAccessor
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
                if (_accessor.HttpContext == null) // TODO: remove test only
                    return "Tenant";
                return accessorHttpContext.User.Claims.Single(c => c.Type == "https://auth.guestline.app/claims/tenant")
                    .Value;
            }
        }
    }
}
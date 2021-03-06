using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Tasks
{
    public interface ITenantAccessor
    {
        string Current { get; }
    }

    class TenantAccessor : ITenantAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public TenantAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Current
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
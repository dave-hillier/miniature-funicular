using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Users
{
    public interface ITenantAccessor
    {
        string Current { get; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal class TenantAccessor : ITenantAccessor
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
                var context = _accessor.HttpContext;
                return context.User.Claims.Single(c => c.Type == "https://auth.guestline.app/claims/tenant").Value;
            }
        }
    }


}
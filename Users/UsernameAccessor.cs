using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Users
{
    public interface IUsernameAccessor
    {
        string Current { get; }
    }
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class UsernameAccessor : IUsernameAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public UsernameAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Current
        {
            get
            {
                var context = _accessor.HttpContext;
                return context.User.Claims.Single(c => c.Type == "https://auth.guestline.app/claims/username").Value;
            }
        }
    }
}
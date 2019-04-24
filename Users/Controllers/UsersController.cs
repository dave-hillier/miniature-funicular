using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Resources;
using HalHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Users.Model;

namespace Users.Controllers
{
    // TODO: this needs to be updated from Events of some sort - writes should come from Rezlynx
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUsernameAccessor _usernameAccessor;

        public UsersController(ApplicationDbContext dbContext, IUsernameAccessor usernameAccessor)
        {
            _dbContext = dbContext;
            _usernameAccessor = usernameAccessor;
        }

        [Authorize("read:users")]
        [HttpGet]
        public ActionResult<ResourceBase> ListForTenant()
        {
            var users = UsersIncludingGroups().Select(u => CreateUser(u));
            var userResources = users.ToArray<ResourceBase>();
            
            var response = new ResourceBase("/api/users")
                .AddEmbedded("data", userResources)
                .AddLink("current", "/api/users/@me"); 
            
            return Ok(response);
        }

        [Authorize("read:users")]
        [HttpGet("@me")]
        public IActionResult GetMe()
        {
            var name = _usernameAccessor.Current;
            var currentUser = UsersIncludingGroups().Single(u => u.Username == name);

            return Redirect($"/api/users/{currentUser.Id}");
        }

        [Authorize("read:users")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource>> Get(string id)
        {
            var user = await UsersIncludingGroups().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();
            return new OkObjectResult(CreateUser(user));
        }

        private IIncludableQueryable<User, Group> UsersIncludingGroups()
        {
            return _dbContext.Users
                .Include(u => u.UserGroups)
                .ThenInclude(post => post.Group);
        }

        private static UserResource CreateUser(User user)
        {
            var userResource = new UserResource($"/api/users/{user.Id.ToLowerInvariant()}") { DisplayName = user.Username };
            var userGroupResources = user.UserGroups.Select(CreateUserGroupResource).ToList();            
            userResource.AddEmbedded("groups", userGroupResources);           
            return userResource;
        }

        private static ResourceBase CreateUserGroupResource(UserGroup ug)
        {
            return new UserGroupResource($"/api/group/{ug.Group.Id.ToLowerInvariant()}") { DisplayName = ug.Group.DisplayName };
        }
    }
}

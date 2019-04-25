using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Resources;
using HalHelper;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<ResourceBase>> ListForTenant()
        {
            var users = Users.Select(u => CreateUser(u));
            var userResources = await users.ToListAsync();
            
            var response = new ResourceBase("/api/users")
                .AddEmbedded("data", userResources)
                .AddLink("current", "/api/users/@me"); 
            
            return Ok(response);
        }

        [Authorize("read:users")]
        [HttpGet("@me")]
        public async Task<ActionResult> GetMe()
        {
            var name = _usernameAccessor.Current;
            
            var currentUser = await Users.SingleOrDefaultAsync(u => u.Username == name);
            if (currentUser == null)
                return BadRequest();
            
            return Redirect($"/api/users/{currentUser.Id}"); // TODO: consider returning an embedded resource
        }

        [Authorize("read:users")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource>> Get(string id)
        {
            var user = await Users.SingleOrDefaultAsync(u => u.Id == id); 
            if (user == null)
                return NotFound();
            return new OkObjectResult(CreateUser(user));
        }

        private IQueryable<User> Users
        {
            get {
                return _dbContext.Users
                    .Include(u => u.UserGroups)
                    .ThenInclude(post => post.Group);
            }
        }
        

        private static ResourceBase CreateUser(User user)
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

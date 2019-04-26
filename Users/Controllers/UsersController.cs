using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Resource>> ListForTenant()
        {
            var users = Users.Select(u => CreateUser(u));
            var userResources = await users.ToListAsync();
            
            var response = new Resource("/api/users")
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
            
            return Redirect($"/api/users/{currentUser.Id}"); 
        }

        [Authorize("read:users")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(string id)
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
        

        private static Resource CreateUser(User user)
        {
            var userResource = new Resource($"/api/users/{user.Id.ToLowerInvariant()}") { State = user };
            var userGroupResources = user.UserGroups.Select(CreateUserGroupResource).ToList();            
            userResource.AddEmbedded("groups", userGroupResources);           
            return userResource;
        }

        private static Resource CreateUserGroupResource(UserGroup ug)
        {
            return new Resource($"/api/group/{ug.Group.Id.ToLowerInvariant()}") { State = ug.Group };
        }
    }
}

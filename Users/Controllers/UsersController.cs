using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Resources;
using HalHelper;
using Users.Model;

namespace Users.Controllers
{
    // TODO: this needs to be updated from Events of some sort - writes should come from Rezlynx

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [Authorize("read:users")]
        [HttpGet]
        public ActionResult<ResourceBase> ListForTenant()
        {
            var response1 = new ResourceBase("/api/users");
            var users = _dbContext.Users.Select(u => CreateUser(u.Id, u.Username));
            var userResources = users.ToArray<ResourceBase>();
            var response = response1.AddEmbedded("data", userResources);
            return new OkObjectResult(response);
        }

        [Authorize("read:users")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResource>> Get(string id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return new OkObjectResult(CreateUser(user.Id, user.Username));
        }

        private static UserResource CreateUser(string id, string name)
        {
            return new UserResource($"/api/users/{id.ToLowerInvariant()}") { DisplayName = name };
        }

    }
}

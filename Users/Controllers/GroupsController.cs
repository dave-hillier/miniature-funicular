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
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public GroupsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [Authorize("read:users")]
        [HttpGet]
        public async Task<ActionResult<ResourceBase>> ListGroups()
        {
            var groupResources = _dbContext.Groups.Select(group => new UserGroupResource($"/api/groups/{group.Id}")
                {DisplayName = group.DisplayName}).Cast<ResourceBase>();
            var list = await groupResources.ToListAsync();
            var resourceCollection = new ResourceBase("/api/groups")
                .AddEmbedded("data", list);
            return Ok(resourceCollection);
        }

        [Authorize("read:users")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroupResource>> Get(string id)
        {
            var group = await _dbContext.Groups.FindAsync(id);
            if (group == null)
                return NotFound();
            var userGroupResource = new UserGroupResource($"/api/groups/{group.Id}")
            {
                DisplayName = group.DisplayName
            };
            return new OkObjectResult(userGroupResource);
        }
    }
}

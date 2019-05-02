using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> ListGroups()
        {
            var groupResources = _dbContext.Groups.Select(group => new Resource($"/api/groups/{group.Id}")
                {State = group}).Cast<Resource>();
            var list = await groupResources.ToListAsync();
            var resourceCollection = new Resource("/api/groups")
                .AddEmbedded("data", list);
            return Ok(resourceCollection);
        }

        [Authorize("read:users")]
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> Get(string id)
        {
            var group = await _dbContext.Groups.FindAsync(id);
            if (group == null)
                return NotFound();
            var userGroupResource = new Resource($"/api/groups/{group.Id}")
            {
                State = group
            };
            return new OkObjectResult(userGroupResource);
        }
    }
}

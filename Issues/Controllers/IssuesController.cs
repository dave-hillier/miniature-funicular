using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Issues.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Issues.Resources;
using Microsoft.EntityFrameworkCore;

namespace Issues.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IssuesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [Authorize("read:issues")]
        [HttpGet]
        public async Task<ActionResult<ResourceBase>> GetAllIssues()
        {
            
            var issues = await _applicationDbContext.Issues.ToListAsync();
            
            return new ResourceBase("/api/issues")
                .AddEmbedded("data", issues.Select(CreateIssue)
                    .ToList());
        }

        private static ResourceBase CreateIssue(Issue issue)
        {
            var resource = new IssueResource($"/api/issues/{issue.Id}")
            {
                Title = issue.Title,
                Status = issue.Status,
                Updated = issue.Updated,
                Created = issue.Created,
                Location = issue.Location,
                Category = issue.Category,
                Description = issue.Description,
                Resolved = issue.Resolved
            };
            // TODO: image, assignee
            return resource;
        }

        [Authorize("read:issues")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResourceBase>> Get(string id)
        {
            var issues = await _applicationDbContext.Issues.FindAsync(id);
            if (issues == null)
                return NotFound();
            return Ok(CreateIssue(issues));
        }

        [Authorize("write:issues")]
        [HttpPost]
        public void Create( [FromBody]IssueResource resource)
        {

        }

        [Authorize("write:issues")]
        [HttpPut("{id}")]
        public ActionResult Update(string id, [FromBody]IssueResource resource)
        {
            return null;
        }

        [Authorize("write:issues")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(string id, [FromBody]JsonPatchDocument<IssueResource> resource)
        {
            return null;
        }

        [Authorize("write:issues")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var toRemove = await _applicationDbContext.Issues.FindAsync(id);
            if (toRemove == null)
                return NotFound();
            _applicationDbContext.Issues.Remove(toRemove);
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

    }
}

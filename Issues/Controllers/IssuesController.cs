using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Issues.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Issues.Resources;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

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
        public async Task<ActionResult> Create([FromBody]IssueResource resource)
        {
            var resourceState = CreateResourceState(resource);
            _applicationDbContext.Issues.Add(resourceState);
            await _applicationDbContext.SaveChangesAsync();
            return Created($"/api/issues/{resourceState.Id}", new {}); // TODO: what should the body be here
        }

        private static Issue CreateResourceState(IssueResource resource)
        {
            var resourceState = new Issue()
            {
                Title = resource.Title,
                Category = resource.Category,
                Status = resource.Status,
            };
            return resourceState;
        }

        [Authorize("write:issues")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody]IssueResource resource)
        {
            var resourceState = CreateResourceState(resource);
            resourceState.Id = id;
            resourceState.Tenant = "Tenant";
            
            _applicationDbContext.Update(resourceState);
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:issues")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatch(string id, [FromBody]IssueResource resource)
        {
            var resourceState = CreateResourceState(resource);
            resourceState.Id = id;
            resourceState.Tenant = "Tenant";
            
            _applicationDbContext.Attach(resourceState).Property(x => x.Title).IsModified = true; // TODO: more properties
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
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

        // TODO: attachments endpoint
    }
}

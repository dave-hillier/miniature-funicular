using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Issues.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Resource>> GetAllIssues()
        { 
            var issues = await _applicationDbContext.Issues.ToListAsync();
            
            return new Resource("/api/issues")
                .AddEmbedded("data", issues.Select(CreateIssue).ToList());
        }

        private static Resource CreateIssue(Issue issue)
        {
            var resource = new Resource($"/api/issues/{issue.Id}")
            {
                State = issue
            };
            // TODO: image, assignee
            return resource;
        }

        [Authorize("read:issues")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(string id)
        {
            var issues = await _applicationDbContext.Issues.FindAsync(id);
            if (issues == null)
                return NotFound();
            return Ok(CreateIssue(issues));
        }

        [Authorize("write:issues")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody]Issue resource)
        {            
            _applicationDbContext.Issues.Add(resource);
            resource.Tenant = "Tenant";
            await _applicationDbContext.SaveChangesAsync();
            return Created($"/api/issues/{resource.Id}", new {}); // TODO: what should the body be here
        }


        [Authorize("write:issues")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody]Issue resource)
        {            
            resource.Id = id;
            resource.Tenant = "Tenant"; // TODO: in here?
            
            _applicationDbContext.Update(resource);
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:issues")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatch(string id, [FromBody]Issue resource)
        {
            
            resource.Id = id;
            resource.Tenant = "Tenant";
            
            _applicationDbContext
                .Attach(resource)
                .Property(x => x.Title).IsModified = true; // TODO: more properties
            
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

        // TODO: 
        // TODO: attachments endpoint
    }
}

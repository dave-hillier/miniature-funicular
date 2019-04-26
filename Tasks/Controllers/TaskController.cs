using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasks.Model;
using Tasks.Resources;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITenantAccessor _tenantAccessor;

        public TasksController(ApplicationDbContext applicationDbContext, ITenantAccessor tenantAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _tenantAccessor = tenantAccessor;
        }
        
        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(string id)
        {
            var task = await _applicationDbContext.Tasks
                .Include(t => t.Children).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return NotFound();
            return new OkObjectResult(task.ToResource());
        }

        [Authorize("write:tasks")]
        [HttpPost("{parentId}")]
        public async Task<ActionResult> CreateSubTask(string parentId, [FromBody] TaskModel resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            var parentTask = await _applicationDbContext.Tasks.FindAsync(parentId);
            resource.Tenant = _tenantAccessor.Current;
            resource.Parent = parentTask;
            _applicationDbContext.Tasks.Add(resource);            
            await _applicationDbContext.SaveChangesAsync();    
            return Created($"/api/tasks/{resource.Id}", new {});
        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody]TaskModel resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            resource.Id = id;
            
            _applicationDbContext.Update(resource);
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatch(string id, [FromBody]TaskModel resource)
        {
            var px = resource.GetType()
                .GetProperties()
                .Where(prop => prop.GetValue(resource, null) != null) // TODO: omit defaults...
                .Select(prop => prop.Name)
                .Where(prop => prop != "Id");
            
            resource.Tenant = _tenantAccessor.Current;
            resource.Id = id;
            
            var attached = _applicationDbContext.Attach(resource);
            
            foreach (var prop in px)
            {
                attached.Property(prop).IsModified = true;
            }
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:tasks")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id) // TODO: test
        {
            var toRemove = await _applicationDbContext.Tasks.FindAsync(id);
            if (toRemove == null)
                return NotFound();
            _applicationDbContext.Tasks.Remove(toRemove);
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

    }
}

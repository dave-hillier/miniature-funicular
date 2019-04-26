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
        [HttpPost("{id}")]
        public async Task<ActionResult> CreateSubTask(string id, [FromBody] TaskModel resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            // TODO: 
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody]TaskModel resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            resource.Id = id;
            // TODO: 
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatch(string id, [FromBody]TaskModel resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            resource.Id = id;
            // TODO: 
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

using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        public TasksController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
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
        public ActionResult CreateSubTask(string id, [FromBody] TaskModel resource)
        {
            // TODO: 
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public ActionResult Update(string id, [FromBody]TaskModel resource)
        {
            // TODO: 
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(string id, [FromBody]TaskModel resource)
        {
            // TODO: 
            return null;
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

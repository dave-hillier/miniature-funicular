using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Tasks.Model;
using Tasks.Resources;

namespace Tasks.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITenantAccessor _tenantAccessor;

        public ListsController(ApplicationDbContext applicationDbContext, ITenantAccessor tenantAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _tenantAccessor = tenantAccessor;
        }
        
        [Authorize("read:tasks")]
        [HttpGet]
        [Produces("application/hal+json")]
        public ActionResult<Resource> List()
        {
            var lists = GetLists()
                .Select(ResourceFactory.ToResource);
            
            var resource = new Resource("/api/lists")
                .AddEmbedded("data", lists.ToList());
            
            return Ok(resource);
        }

        private IIncludableQueryable<TaskList, List<TaskModel>> GetLists()
        {
            return _applicationDbContext.List
                .Include(l => l.Tasks)
                .ThenInclude(t => t.Children);
        }


        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> Get(string id)
        {
            var list = await GetLists().FirstOrDefaultAsync(l => l.Id == id);
            if (list == null)
                return NotFound();
            
            return Ok(list.ToResource());
        }

        [Authorize("write:tasks")]
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult> CreateList([FromBody] TaskList resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            _applicationDbContext.List.Add(resource);            
            await _applicationDbContext.SaveChangesAsync();
            return Created($"/api/lists/{resource.Id}", new {}); // TODO: what should the body be here
        }

        [Authorize("write:tasks")]
        [HttpPost("{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult> AddNewTaskToList(string id, [FromBody] TaskModel resource)
        {
            var list = await GetLists().FirstOrDefaultAsync(l => l.Id == id);
            if (list == null)
                return BadRequest();
            
            resource.Tenant = _tenantAccessor.Current;
            resource.ParentTaskList = list;
            
            _applicationDbContext.Tasks.Add(resource);            
            await _applicationDbContext.SaveChangesAsync();    
            return Created($"/api/tasks/{resource.Id}", new {}); // TODO: what should the body be here

        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateList(string id, [FromBody]TaskList resource)
        {
            var taskList = await _applicationDbContext.List.FindAsync(id); 
            if (taskList == null)
                return NotFound();

            taskList.Title = resource.Title;

            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdatePatch(string id, [FromBody]TaskList resource)
        {
            var taskList = await _applicationDbContext.List.FindAsync(id); 
            if (taskList == null)
                return NotFound();

            if (resource.Title != null) taskList.Title = resource.Title;

            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:tasks")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {          
            var toRemove = await _applicationDbContext.List.FindAsync(id); 
            if (toRemove == null)
                return NotFound();
            _applicationDbContext.List.Remove(toRemove);
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

    }
}

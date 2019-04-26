using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        public ListsController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        [Authorize("read:tasks")]
        [HttpGet]
        public ActionResult<Resource> List()
        {
            var lists = GetLists()
                .Select(ResourceFactory.ToResource);
            
            var Resource = new Resource("/api/lists")
                .AddEmbedded("data", lists.ToList());
            
            return Ok(Resource);
        }

        private IIncludableQueryable<TaskList, List<TaskModel>> GetLists()
        {
            return _applicationDbContext.List
                .Include(l => l.Tasks)
                .ThenInclude(t => t.Children);
        }


        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(string id)
        {
            var list = await GetLists().FirstOrDefaultAsync(l => l.Id == id);
            if (list == null)
                return NotFound();
            
            return Ok(list.ToResource());
        }

        [Authorize("write:tasks")]
        [HttpPost]
        public void CreateList([FromBody] TaskModel resource)
        {
            // TODO: 
        }

        [Authorize("write:tasks")]
        [HttpPost("{id}")]
        public void AddNewTaskToList(string id, [FromBody] TaskModel resource)
        {
            // TODO: 
        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public ActionResult UpdateList(string id, [FromBody]TaskList resource)
        {
            // TODO: 
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(string id, [FromBody]TaskList resource)
        {
            // TODO: 
            return null;
        }

        [Authorize("write:tasks")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id) // TODO: test
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

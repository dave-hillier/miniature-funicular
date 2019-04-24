using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<ResourceBase> List()
        {
            var lists = GetLists()
                .Select(ResourceFactory.CreateTaskList);
            var resourceBase = new ResourceBase("/api/lists")
                .AddEmbedded("data", lists.ToList());
            return Ok(resourceBase);
        }

        private IIncludableQueryable<TaskList, List<TaskModel>> GetLists()
        {
            return _applicationDbContext.List
                .Include(l => l.Tasks)
                .ThenInclude(t => t.Children);
        }


        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        public ActionResult<TaskListResource> Get(string id)
        {
            var list = GetLists().FirstOrDefault(l => l.Id == id);
            if (list == null)
                return NotFound();
            
            return Ok(ResourceFactory.CreateTaskList(list));
        }

        [Authorize("write:tasks")]
        [HttpPost]
        public void Create([FromBody] string title)
        {

        }

        [Authorize("write:tasks")]
        [HttpPost("{id}")]
        public void AppendTask(int id, [FromBody] TaskResource title)
        {

        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody]TaskListResource resource)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(int id, [FromBody]JsonPatchDocument<TaskListResource> resource)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return null;
        }

    }
}

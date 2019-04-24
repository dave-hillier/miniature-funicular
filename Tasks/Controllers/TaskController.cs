using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tasks.Resources;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        public ActionResult<TaskResource> Get(int id)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPost("{id}")]
        public ActionResult CreateSubTask(int id, [FromBody] TaskResource title)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody]TaskResource resource)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(int id, [FromBody]JsonPatchDocument<TaskResource> resource) 
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

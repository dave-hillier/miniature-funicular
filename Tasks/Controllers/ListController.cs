using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tasks.Resources;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        [Authorize("read:tasks")]
        [HttpGet]
        public ActionResult<HalResourceBase> List()
        {
            return null;
        }

        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        public ActionResult<ListResource> Get(int id)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPost]
        public void Create([FromBody] string title)
        {

        }

        [Authorize("write:lists")]
        [HttpPost("{id}")]
        public void AppendTask(int id, [FromBody] TaskResource title)
        {

        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody]ListResource resource)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(int id, [FromBody]JsonPatchDocument<ListResource> resource)
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

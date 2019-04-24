using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Issues.Resources;

namespace Issues.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        [Authorize("read:tasks")]
        [HttpGet]
        public ActionResult<HalResourceBase> Issue()
        {
            return null;
        }

        [Authorize("read:tasks")]
        [HttpGet("{id}")]
        public ActionResult<IssueResource> Get(int id)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPost]
        public void Create([FromBody] string title)
        {

        }

        [Authorize("write:Issues")]
        [HttpPost("{id}")]
        public void AppendTask(int id, [FromBody] IssueResource title)
        {

        }

        [Authorize("write:tasks")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody]IssueResource resource)
        {
            return null;
        }

        [Authorize("write:tasks")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(int id, [FromBody]JsonPatchDocument<IssueResource> resource)
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

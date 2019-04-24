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
        [Authorize("read:issues")]
        [HttpGet]
        public ActionResult<HalResourceBase> Issue()
        {
            return null;
        }

        [Authorize("read:issues")]
        [HttpGet("{id}")]
        public ActionResult<IssueResource> Get(int id)
        {
            return null;
        }

        [Authorize("write:issues")]
        [HttpPost]
        public void Create([FromBody] string title)
        {

        }

        [Authorize("write:issues")]
        [HttpPost("{id}")]
        public void AppendTask(int id, [FromBody] IssueResource title)
        {

        }

        [Authorize("write:issues")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody]IssueResource resource)
        {
            return null;
        }

        [Authorize("write:issues")]
        [HttpPatch("{id}")]
        public ActionResult UpdatePatch(int id, [FromBody]JsonPatchDocument<IssueResource> resource)
        {
            return null;
        }

        [Authorize("write:issues")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return null;
        }

    }
}

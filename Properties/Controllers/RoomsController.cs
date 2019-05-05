using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Mvc;
using Properties.Model;

namespace Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetProperty(int id)
        {
            var rooms = await _dbContext.Rooms.FindAsync(id);
            if (rooms == null)
                return NotFound();
            
            return Ok();
        }
    }
}
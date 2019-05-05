using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Mvc;
using Properties.Model;

namespace Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomTypesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetProperty(int id)
        {
            var roomTypes = await _dbContext.Rooms.FindAsync(id);
            if (roomTypes == null)
                return NotFound();
            
            return Ok();
        }
    }
}
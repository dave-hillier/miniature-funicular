using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Authorization;
using Properties.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IFileStorage _fileStorage;

        public PropertiesController(ApplicationDbContext applicationDbContext,
            ITenantAccessor tenantAccessor,
            IFileStorage fileStorage)
        {
            _applicationDbContext = applicationDbContext;
            _tenantAccessor = tenantAccessor;
            _fileStorage = fileStorage;
        }

        /*[HttpPost]
        [Authorize("create:property")]
        [Consumes("application/json")]
        public async Task<ActionResult> CreateProperty([FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Created("/", new {});
        }
        
        [HttpPut("{thing}")]
        [Authorize("update:property")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateProperty(string thing, [FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Ok();
        }

        [HttpPatch("{thing}")]
        [Authorize("update:property")]
        [Consumes("application/json")]
        public async Task<ActionResult> PatchProperty(string thing, [FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Ok();
        }
        
        [HttpDelete("{thing}")]
        [Authorize("delete:property")] 
        [Consumes("application/json")]
        public async Task<ActionResult> DeleteProperty(string thing, [FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Ok();
        }*/

        [HttpGet("current/{tenant}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetLatestVersionProperties(string tenant)
        {
            var properties = await GetQueryable(tenant)
                .ToListAsync();

            if (!properties.Any())
                return NotFound();

            var resources = properties
                .Select(ToResource)
                .ToList();

            var resource = new Resource($"/api/properties/current/{tenant}")
                .AddEmbedded("properties", resources);

            return Ok(resource);
        }

        private Resource ToResource(Property property)
        {
            var imageLinks = property.Current.Images.Select(l => new Link
            {
                Href = l.Href,
            }).ToArray();

            return new Resource($"/api/properties/current/{property.Tenant}/{property.Id}")
                {
                    State = property.Current,
                }
                .AddLinks("images", imageLinks)
                .AddEmbedded("rooms", property.Current.Rooms.Select(ToResource).ToList())
                .AddEmbedded("roomsTypes", property.Current.RoomTypes.Select(ToResource).ToList());
        }

        private static Resource ToResource(Room room)
        {
            return new Resource($"/api/rooms/{room.Id}")
            {
                State = room
            }.AddLink("roomType", $"/api/roomTypes/{room.RoomTypeId}");
        }
        
        private Resource ToResource(RoomType roomType)
        {
            var images = roomType.Images.Select(l => new Link {  Href = l.Href  }).ToList();
            var resources = roomType.SubRooms.Select(ToResource).ToList(); // TODO: Actually query for them

            
            return new Resource($"/api/roomTypes/{roomType.Id}") 
                { 
                    State = new
                    {
                        roomType.Name,
                        roomType.Description,
                        roomType.Amenities,
                        Tags = roomType.Tags.Select(t => t.Tag).ToList(),
                    }
                }
                .AddLinks("images", images.ToArray())
                .AddEmbedded("subRooms", resources);
        }


        /*[HttpGet("current/{tenant}/{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetCurrentVersionProperty(string tenant, string id) 
        {
            var property = await GetQueryable(tenant)
                .SingleOrDefaultAsync(p => p.Id == id);
            
            if (property == null)
                return NotFound();
            
            var resource = new Resource($"/api/properties/current/{tenant}/{property.Id}");
            
            return Ok(resource);
        }

                
        [HttpGet("version/{version}")]
        [Produces("application/hal+json")] 
        public async Task<ActionResult<Resource>> GetProperty(string version)
        {
            var property = await GetQueryableVersion(version)
                .FirstOrDefaultAsync();            
                     
            if (property == null)
                return NotFound();

            var resource = new Resource($"/api/properties/version/{property.Id}")
            {
               State = property
            };
            
            return Ok(resource);
        }*/

        private IQueryable<PropertyVersion> GetQueryableVersion(string version)
        {
            return _applicationDbContext.PropertyVersion
                .Include(pv => pv.Name)
                .Include(pv => pv.Description)
                .Include(pv => pv.Rooms).ThenInclude(rt => rt.Name)
                .Include(pv => pv.Rooms).ThenInclude(rt => rt.Description)
                .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Name)
                .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Description)
                .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Amenities)
                .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.SubRooms).ThenInclude(rt => rt.SubRooms)                    
                .Where(p => p.Id == version);
        }

        private IQueryable<Property> GetQueryable(string tenant)
        {
            return _applicationDbContext.Properties
                .Include(p => p.Current).ThenInclude(pv => pv.Name)
                .Include(p => p.Current).ThenInclude(pv => pv.Description)
                .Include(p => p.Current).ThenInclude(pv => pv.Rooms).ThenInclude(rt => rt.Name)
                .Include(p => p.Current).ThenInclude(pv => pv.Rooms).ThenInclude(rt => rt.Description)
                .Include(p => p.Current).ThenInclude(pv => pv.RoomTypes).ThenInclude(rt => rt.Name)
                .Include(p => p.Current).ThenInclude(pv => pv.RoomTypes).ThenInclude(rt => rt.Description)
                .Include(p => p.Current).ThenInclude(pv => pv.RoomTypes).ThenInclude(rt => rt.Amenities)
                .Include(p => p.Current).ThenInclude(pv => pv.RoomTypes).ThenInclude(rt => rt.SubRooms).ThenInclude(rt => rt.SubRooms) // TODO: encapsulate room includes
                .Where(p => p.Tenant == tenant);
        }

    }
}

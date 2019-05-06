using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Microsoft.AspNetCore.Authorization;
using Properties.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Properties.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITenantAccessor _tenantAccessor;

        public PropertiesController(
            ApplicationDbContext applicationDbContext,
            ITenantAccessor tenantAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _tenantAccessor = tenantAccessor;
        }

        [HttpPost("/current/{tenant}/{id}/roomTypes")]
        [Authorize("update:property")]
        [Consumes("application/json")]
        public async Task<ActionResult> AddRoomType(string tenant, string id, [FromBody]RoomType body)
        {
            if (tenant != _tenantAccessor.Current)
                return Unauthorized();
            
            var property = await GetPropertyAsync(tenant, id);

            if (property == null)
                return NotFound();

            _applicationDbContext.RoomTypes.Add(body);           

            return Created($"/api/roomTypes/{body.Id}", new {});
        }
       
        [HttpPost("current/{tenant}/{id}/{roomTypeId}/rooms")] // TODO: add roomtype?
        [Authorize("update:property")]
        [Consumes("application/json")]
        public async Task<ActionResult> AddRooms(string tenant, string id, [FromBody]Room body) 
        {
            if (tenant != _tenantAccessor.Current)
                return Unauthorized();


            await _applicationDbContext.SaveChangesAsync();
            
            return Created($"/api/rooms/{body.Id}", new {}); // TODO
        }

        // Lookup the hotels for a given tenant
        [HttpGet("current/{tenant}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetLatestVersionProperties(string tenant)
        {
            var properties = await GetPropertyByTenant(tenant).ToListAsync();

            if (!properties.Any())
                return NotFound();

            var props = properties .Select(p => 
                    new Resource($"/api/properties/current/{tenant}/{p.Id}")
                        .AddLink("direct", $"/api/properties/versions/{tenant}/{p.Current.Version}")).ToArray();
            
            var resource = new Resource($"/api/properties/current/{tenant}")
                .AddEmbedded("properties", props);

            return Ok(resource);
        }

        [HttpGet("current/{tenant}/{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetLatestVersionProperties(string tenant, string id)
        {
            var property = await GetPropertyAsync(tenant, id);

            if (property == null)
                return NotFound();

            return Redirect($"/api/properties/versions/{property.Tenant}/{property.Current.Version}");
        }

        // Get property, immutable, cacheable
        [HttpGet("versions/{tenant}/{version}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetProperty(string tenant, string version)
        {
            var property = await GetQueryablePropertyVersion(tenant, version)
                .SingleOrDefaultAsync();
            
            if (property == null)
                return NotFound();

            return Ok(property.ToResource());
        }
        
        private IQueryable<PropertyVersion> GetQueryablePropertyVersion(string tenant, string version)
        {
            return _applicationDbContext.PropertyVersion
                    .Where(p => p.Tenant == tenant && p.Version == version)
                    .Include(pv => pv.Name)
                    .Include(pv => pv.Description)
                    .Include(pv => pv.Rooms).ThenInclude(rt => rt.Name)
                    .Include(pv => pv.Rooms).ThenInclude(rt => rt.Description)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Name)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Description)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Amenities)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.SubRooms).ThenInclude(rt => rt.SubRooms);
        }
        
        private Task<Property> GetPropertyAsync(string tenant, string id)
        {
            return GetPropertyByTenant(tenant)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }


        private IQueryable<Property> GetPropertyByTenant(string tenant)
        {
            return _applicationDbContext.Properties
                .Include(p => p.Current)
                .Where(p => p.Tenant == tenant);
        }
    }
}

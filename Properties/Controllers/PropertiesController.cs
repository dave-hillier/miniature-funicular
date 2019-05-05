using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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

        /*
        [HttpPost]
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

        // Lookup the hotels for a given tenant
        [HttpGet("current/{tenant}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetLatestVersionProperties(string tenant)
        {
            var properties = await _applicationDbContext.Properties
                .Include(p => p.Current)
                .Where(p => p.Tenant == tenant)
                .ToListAsync();

            if (!properties.Any())
                return NotFound();

            var urls = properties
                .Select(p => new Link { Href =$"/api/properties/{p.Tenant}/{p.Current.Id}" })
                .ToArray();

            var resource = new Resource($"/api/properties/current/{tenant}")
                .AddLinks("properties", urls);

            return Ok(resource);
        }

        // Get property
        [HttpGet("{tenant}/{id}")]
        [Produces("application/hal+json")]
        public async Task<ActionResult<Resource>> GetProperty(string tenant, string id)
        {
            var property = await GetQueryableVersion(tenant, id)
                .SingleOrDefaultAsync();
            
            if (property == null)
                return NotFound();

            return Ok(ToResource(property));
        }
        
        private static Resource ToResource(PropertyVersion propertyVersion)
        {
            var imageLinks = propertyVersion.Images.Select(l => new Link { Href = l.Href }).ToArray();

            return new Resource($"/api/properties/{propertyVersion.Tenant}/{propertyVersion.Id}")
                {
                    State = propertyVersion,
                }
                .AddLinks("images", imageLinks)
                .AddEmbedded("rooms", propertyVersion.Rooms.Select(ResourceExtensions.ToResourceX).ToList())
                .AddEmbedded("roomsTypes", propertyVersion.RoomTypes.Select(ResourceExtensions.ToResource).ToList());
        }

        private IQueryable<PropertyVersion> GetQueryableVersion(string tenant, string version)
        {
            return _applicationDbContext.PropertyVersion
                    .Where(p => p.Tenant == tenant && p.Id == version)
                    .Include(pv => pv.Name)
                    .Include(pv => pv.Description)
                    .Include(pv => pv.Rooms).ThenInclude(rt => rt.Name)
                    .Include(pv => pv.Rooms).ThenInclude(rt => rt.Description)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Name)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Description)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.Amenities)
                    .Include(pv => pv.RoomTypes).ThenInclude(rt => rt.SubRooms).ThenInclude(rt => rt.SubRooms);
        }
    }
}

using System.Linq;
using System.Linq.Expressions;
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
    class PropertiesController : ControllerBase
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

        [HttpPost]
        [Authorize("create:property")]
        [Consumes("application/json")]// TODO: mark up the rest Consumes 
        public async Task<ActionResult> CreateProperty([FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Created("/", new {});
        }
        
        [HttpPut("{thing}")]
        [Authorize("update:property")]
        [Consumes("application/json")]// TODO: mark up the rest Consumes 
        public async Task<ActionResult> UpdateProperty(string thing, [FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Ok();
        }

        [HttpPatch("{thing}")]
        [Authorize("update:property")]
        [Consumes("application/json")]// TODO: mark up the rest Consumes 
        public async Task<ActionResult> PatchProperty(string thing, [FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Ok();
        }
        
        [HttpDelete("{thing}")]
        [Authorize("delete:property")] 
        [Consumes("application/json")]// TODO: mark up the rest Consumes 
        public async Task<ActionResult> DeleteProperty(string thing, [FromBody]PropertyRequest body)
        {
            var tenant = _tenantAccessor.Current;
            // TODO: 
            return Ok();
        }

        [HttpGet("/current/{tenant}")]
        [Produces("application/hal+json")] // TODO: mark the rest
        public async Task<ActionResult<Resource>> GetLatestVersionProperties(string tenant)
        {
            var properties = await GetQueryable(tenant)
                .ToListAsync();
            
            var resources = properties
                .Select(p => new Resource($"/api/properties/current/{tenant}/{p.Id}")
                {
                    State = p
                })
                .ToList();

            var resource = new Resource($"/api/properties/current/{tenant}")
                .AddEmbedded("data", resources);
            
            return Ok(resource);
        }



        [HttpGet("/current/{tenant}/{id}")]
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

                
        [HttpGet("/version/{version}")]
        [Produces("application/hal+json")] // TODO: mark the rest
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
        }

        private IQueryable<PropertyVersion> GetQueryableVersion(string version)
        {
            return _applicationDbContext.PropertyVersion
                .Include(p => p.Address)
                .ThenInclude(a => a.Lines)
                .Include(p => p.Images)
                .Include(p => p.Description)
                .Include(p => p.ManagementInfo)
                .ThenInclude(m => m.Address)
                .ThenInclude(a => a.Lines)
                .Include(p => p.ManagementInfo)
                .ThenInclude(m => m.PhoneInfo)
                .Include(p => p.Name)
                .Where(p => p.Id == version);
        }
        
        private IQueryable<PropertyThing> GetQueryable(string tenant)
        {
            return _applicationDbContext.Properties          
                .Include(p => p.Current)
                .ThenInclude(p => p.Address)
                .ThenInclude(a => a.Lines)
                .Include(p => p.Current)
                .ThenInclude(p => p.Images)
                .Include(p => p.Current)
                .ThenInclude(p => p.Description)
                .Include(p => p.Current)
                .ThenInclude(p => p.ManagementInfo)
                .ThenInclude(m => m.Address)
                .ThenInclude(a => a.Lines)
                .Include(p => p.Current)
                .ThenInclude(p => p.ManagementInfo)
                .ThenInclude(m => m.PhoneInfo)
                .Include(p => p.Current)
                .ThenInclude(p => p.Name)
                .Where(p => p.Tenant == tenant);
        }

    }
}

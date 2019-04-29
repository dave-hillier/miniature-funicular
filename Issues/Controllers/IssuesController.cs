using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HalHelper;
using Issues.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Issues.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly IFileStorage _fileStorage;

        public IssuesController(ApplicationDbContext applicationDbContext, 
            ITenantAccessor tenantAccessor, 
            IFileStorage fileStorage)
        {
            _applicationDbContext = applicationDbContext;
            _tenantAccessor = tenantAccessor;
            _fileStorage = fileStorage;
        }
        
        [Authorize("read:issues")]
        [HttpGet]
        public async Task<ActionResult<Resource>> GetAllIssues()
        { 
            var issues = await _applicationDbContext.Issues
                .Include(i => i.Images).ToListAsync();
            
            return new Resource("/api/issues")
                .AddEmbedded("data", issues.Select(ToResource).ToList());
        }

        private static Resource ToResource(Issue issue)
        {            
            var images = new Resource($"/api/issues/{issue.Id}/images")
            {
                State = new { data = issue.Images.Select(i => i.ImageUrl) }
            };
            var resource = new Resource($"/api/issues/{issue.Id}")
            {
                State = issue
            }.AddEmbedded("images", images);
            return resource;
        }

        [Authorize("read:issues")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> Get(string id)
        {
            var issues = await _applicationDbContext.Issues
                .Include(i => i.Images)
                .SingleOrDefaultAsync(i => i.Id == id);
            
            if (issues == null)
                return NotFound();
            return Ok(ToResource(issues));
        }

        [Authorize("write:issues")] // TODO: test me
        [HttpPost]
        public async Task<ActionResult> CreateFromForm([FromForm]IssueForm issueForm)
        {
            var tenant = _tenantAccessor.Current;
            var images = await Task.WhenAll(
                issueForm.Images.Select(photo => _fileStorage.StoreAsync(tenant, photo))
            );

            var resource = new Issue
            {
                Tenant = _tenantAccessor.Current,
                Description = issueForm.Description,                
            };
            resource.Images = images.Select(i => new IssueImage {Parent = resource, ImageUrl = i}).ToList();
            _applicationDbContext.Issues.Add(resource);
            await _applicationDbContext.Images.AddRangeAsync(resource.Images);
            await _applicationDbContext.SaveChangesAsync();
            return Created($"/api/issues/{resource.Id}", new {}); // TODO: what should the body be here
        }
        
        [Authorize("write:issues")]
        [HttpPost("/resource")]
        public async Task<ActionResult> Create([FromBody]Issue resource)
        {
            resource.Tenant = _tenantAccessor.Current;
            _applicationDbContext.Issues.Add(resource);            
            await _applicationDbContext.SaveChangesAsync();
            return Created($"/api/issues/{resource.Id}", new {});
        }

        
        [Authorize("write:issues")]
        [HttpPost("{id}/images")]
        public async Task<ActionResult> AddImage(string id, [FromBody]string imageUrl)
        {
            var issue = await _applicationDbContext.Issues.FindAsync(id);
            if (issue == null)
                return NotFound();

            var listImage = new IssueImage {Parent = issue, ImageUrl = imageUrl};
            issue.Images.Add(listImage); // not sure this does anything
            _applicationDbContext.Images.Add(listImage);
         
            await _applicationDbContext.SaveChangesAsync();
            return Created($"/api/issues/{issue.Id}/images", new {});
        }
        
        [Authorize("write:issues")]
        [HttpDelete("{id}/images")]
        public async Task<ActionResult> Delete(string id, [FromBody]string imageUrl)
        {
            var issue = await _applicationDbContext.Issues.FindAsync(id);
            if (issue == null)
                return NotFound();
            
            var image = issue.Images.FirstOrDefault(i => i.ImageUrl == imageUrl);
            issue.Images.Remove(image);            
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:issues")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody]Issue resource)
        {            
            var issue = await _applicationDbContext.Issues.FindAsync(id);
            if (issue == null)
                return NotFound();
            
            issue.Status = resource.Status;
            issue.Category = resource.Category;
            issue.Location = resource.Location;
            issue.Description = resource.Description;
            
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:issues")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatch(string id, [FromBody]Issue resource)
        {
            var issue = await _applicationDbContext.Issues.FindAsync(id);
            if (issue == null)
                return NotFound();

            if (resource.Status != null) issue.Status = resource.Status;
            if (resource.Category != null) issue.Category = resource.Category;
            if (resource.Location != null) issue.Location = resource.Location;
            if (resource.Description != null) issue.Description = resource.Description;

            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize("write:issues")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var toRemove = await _applicationDbContext.Issues.FindAsync(id);
            if (toRemove == null)
                return NotFound();
            
            _applicationDbContext.Issues.Remove(toRemove);
            await _applicationDbContext.SaveChangesAsync();
            return Ok();
        }
    }

    public class IssueForm // TODO: rest of stuff
    {
        [Required]
        [MinLength(10)]
        [DataType(DataType.Text)] // URL?
        public string Location { get; set; } // TODO: multiple
        
        [Required]
        [MinLength(1)]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public IEnumerable<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}

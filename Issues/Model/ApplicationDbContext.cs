using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Issues.Model
{

    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantAccessor _accessor;
        public DbSet<Issue> Issues { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>().HasQueryFilter(b => b.Tenant == _accessor.CurrentTenant); // TODO: omit for testing?
        }
    }
}
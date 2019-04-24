using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Model
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantAccessor _accessor;
        public DbSet<List> List { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().HasQueryFilter(b => b.Tenant == _accessor.CurrentTenant);
            modelBuilder.Entity<List>().HasQueryFilter(b => b.Tenant == _accessor.CurrentTenant);
        }

    }
}
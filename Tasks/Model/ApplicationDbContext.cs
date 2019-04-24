using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Model
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantAccessor _tenantAccessor;
        public DbSet<TaskList> List { get; set; }

        public DbSet<TaskModel> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantAccessor tenantAccessor) : base(options)
        {
            _tenantAccessor = tenantAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>().HasQueryFilter(b => b.Tenant == _tenantAccessor.Current);
            modelBuilder.Entity<TaskList>().HasQueryFilter(b => b.Tenant == _tenantAccessor.Current);
            
            
        }

    }
}
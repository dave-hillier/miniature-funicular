using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Users.Model
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantAccessor _accessor;
        public DbSet<User> Users { get; set; }
        
        public DbSet<Group> Groups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(b =>  b.Tenant == _accessor.CurrentTenant);
            modelBuilder.Entity<Group>().HasQueryFilter(b =>  b.Tenant == _accessor.CurrentTenant);
        }

    }
}
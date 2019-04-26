using Microsoft.EntityFrameworkCore;

namespace Users.Model
{
    public class UserGroup
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string GroupId { get; set; }
        public Group Group { get; set; }
    }
    
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantAccessor _accessor;
        public DbSet<User> Users { get; set; }
        
        public DbSet<Group> Groups { get; set; }
        
        public DbSet<UserGroup> UserGroups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        // TODO: consider https://docs.microsoft.com/en-us/ef/core/querying/related-data#lazy-loading
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(b =>  b.Tenant == _accessor.Current);
            modelBuilder.Entity<Group>().HasQueryFilter(b =>  b.Tenant == _accessor.Current);
            
            // TODO: can this be improved?            
            modelBuilder.Entity<UserGroup>()
                .HasKey(t => new { t.UserId, t.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.Group)
                .WithMany(t => t.UserGroups)
                .HasForeignKey(pt => pt.GroupId);
        }

    }
}
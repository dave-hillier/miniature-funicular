using Microsoft.EntityFrameworkCore;

namespace Users.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Group> Groups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: query filter
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: 
        }

    }
}
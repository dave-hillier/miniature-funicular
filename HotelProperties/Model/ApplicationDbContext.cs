using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Properties.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyVersion> PropertyVersion { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomType> RoomTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Translations>()
                .OwnsMany(p => p.Values);

            modelBuilder.Entity<PropertyVersion>()
                .OwnsMany(p => p.Images);

            modelBuilder.Entity<PropertyVersion>()
                .OwnsMany(p => p.ContactInfos, info =>
                {
                    info.OwnsOne(a => a.Address, address =>
                    {
                        address.OwnsMany(a => a.Lines);
                    });

                    info.OwnsMany(a => a.PhoneInfos);
                });

            modelBuilder.Entity<RoomType>()
                .OwnsMany(rt => rt.Images);
            modelBuilder.Entity<RoomType>()
                .OwnsMany(rt => rt.Tags);

        }
    }
}
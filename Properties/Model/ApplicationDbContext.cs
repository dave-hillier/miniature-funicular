using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{

    class ApplicationDbContext : DbContext
    {
        public DbSet<PropertyThing> Properties { get; set; }
        public DbSet<PropertyVersion> PropertyVersion { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomTag> RoomTags { get; set; }
        public DbSet<OtaAmenity> Amenities { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<LocalizableContent> LocalizableContent { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<AddressLine> AddressLine { get; set; }   
        public DbSet<ManagementInfo> ManagementInfo { get; set; }   

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<AddressLine>().HasKey(l => new { l.ParentId, l.LineNo });             
        }
    }
}
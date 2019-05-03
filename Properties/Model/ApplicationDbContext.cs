using Microsoft.EntityFrameworkCore;

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
                .OwnsMany(p => p.Values, info =>
                {
                    info.HasForeignKey("TranslationsId");
                    info.HasKey("LanguageTag", "TranslationsId");
                });
            
           modelBuilder.Entity<PropertyVersion>()
               .OwnsMany(p => p.Images, image =>
               {
                   image.HasForeignKey("PropertyVersionId");
                   image.Property<int>("Id");
                   image.HasKey( "Id");                   
               })
               .OwnsMany(p => p.ContactInfos, info =>
               {
                   info.HasForeignKey("PropertyVersionId");
                   info.Property<int>("Id");
                   info.HasKey( "Id");
                   
                   info.OwnsOne(a => a.Address, address =>
                   {
                       address.OwnsMany(a => a.Lines, line =>
                       {
                           line.HasForeignKey("AddressId");
                           line.Property<int>("Id");
                           line.HasKey("Id");
                       });
                   });
                   
                   info.OwnsMany(a => a.PhoneInfos, pn =>
                   {
                       pn.HasForeignKey("PhoneInfoId");
                       pn.Property<int>("Id");
                       pn.HasKey("Id");
                   });
               });

           modelBuilder.Entity<RoomType>()
               .OwnsMany(rt => rt.Images, image =>
               {                   
                   image.HasForeignKey("RoomTypeId");
                   image.Property<int>("Id");
                   image.HasKey("Id");
               })
               .OwnsMany(rt => rt.Tags, tag =>
               {
                   tag.HasForeignKey("RoomTypeId");
                   tag.HasKey("Tag");
               });

        }
    }
}
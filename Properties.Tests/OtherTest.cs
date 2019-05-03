using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Properties.Tests
{
    public class OtherTest
    {

        [Owned]
        public class StreetAddress 
        {       
            public string Street { get; set; }
            public string City { get; set; }
        }
        
        public class Distributor
        {
            public string Id { get; set; }
            
            public StreetAddress Address { get; set; }
            public ICollection<StreetAddress> ShippingCenters { get; set; }
        }

        public class Distributor2
        {
            public int Id { get; set; }

            public StreetAddress Address { get; set; }
            public ICollection<StreetAddress> ShippingCenters { get; set; }
        }
        
        public class TestDbContext : DbContext
        {
            public DbSet<Distributor> Distributors { get; set; }
            public DbSet<Distributor2> Distributors2 { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase("Test");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Distributor>().OwnsMany(rt => rt.ShippingCenters, sc =>
                {
                    sc.HasForeignKey("DistributorId");
                    sc.Property<int>("Id");
                    sc.HasKey( "Id");
                }).OwnsOne(rt => rt.Address, sc =>
                {
                    
                });
                modelBuilder.Entity<Distributor2>().OwnsMany(rt => rt.ShippingCenters, sc =>
                {
                    sc.HasForeignKey("Distributor2Id");
                    sc.Property<int>("Id");
                    sc.HasKey( "Id");
                });
            }
        }
     

        [Fact]
        public void TestDb()
        {
            using (var context = new TestDbContext())
            {
                var model = new Distributor
                {
                    Address = new StreetAddress {Street = "X", City="City"},
                    ShippingCenters = new List<StreetAddress>
                    {
                        new StreetAddress {Street = "1", City="City"},
                        new StreetAddress {Street = "2", City="City"} 
                    }
                };                    
                
                var model2 = new Distributor2
                {
                    Address = new StreetAddress {Street = "Y", City="City"},
                    ShippingCenters = new List<StreetAddress>
                    {
                        new StreetAddress {Street = "3", City="City"},
                        new StreetAddress {Street = "4", City="City"} 
                    }
                };
                    
                context.Distributors.Add(model);
                context.Distributors2.Add(model2);
                context.SaveChanges(); 
            }
        }
    }
}
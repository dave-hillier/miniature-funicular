using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Properties.Model;

namespace Properties.Tests
{
    public class PropertiesControllerIntegrationTests
    {
        private readonly HttpClient _testClient;
        private readonly TestServer _testServer;


        public PropertiesControllerIntegrationTests()
        {
            var builder = new WebHostBuilder().ConfigureTest();

            _testServer = new TestServer(builder);
            _testClient = _testServer.CreateClient();

            using (var scope = _testServer.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                Seed(context);
            }
        }

        private void Seed(ApplicationDbContext context)
        {
            var subRoomType = new RoomType
            {
                Name = CreateTranslations("Sub Room Type Name"),
            };
            var roomType = new RoomType
            {
                Name = CreateTranslations("Room Type Name"),
                Tags = new List<RoomTag>
                {
                    new RoomTag {Tag = "Guest"},
                    new RoomTag {Tag = "Double"},
                    new RoomTag {Tag = "Accessible"},
                    new RoomTag {Tag = "Windowless"}
                }, // TODO: serialize as List
                Images = new List<ImageLink1>
                {
                    new ImageLink1 {Href = "https://rl-uk2.azureedge.net/picturemanager/images/OBMNG1/room3.jpg"},
                    new ImageLink1 {Href = "https://rl-uk2.azureedge.net/picturemanager/images/OBMNG1/room5.jpg"}
                },
                SubRooms = new List<RoomType> {subRoomType}
            };
            var otaAmenities = new List<OtaAmenity>
            {
                new OtaAmenity {Id = 33, Parent = roomType},
                new OtaAmenity {Id = 5001, Parent = roomType}
            };
            roomType.Amenities = otaAmenities;

            context.RoomTypes.Add(roomType);

            context.SaveChanges();

            var room = new Room
            {
                RoomTypeId = roomType.Id,
                Name = CreateTranslations("Room Name"),
                Description = CreateTranslations("Room Description")
                
            };

            context.Rooms.Add(room);
            context.SaveChanges();


            var propertyVersion = new PropertyVersion
            {
                Name = CreateTranslations("name"),
                Description = CreateTranslations("description"),
                Tenant = "Tenant",
                Category = "Hotel",
                Images = new List<ImageLink>
                {
                    new ImageLink {Href = "https://rl-uk2.azureedge.net/picturemanager/images/OBMNG2/hotel1.jpg"},
                    new ImageLink {Href = "https://rl-uk2.azureedge.net/picturemanager/images/OBMNG1/Hotel1.JPG"}
                },
                Rooms = new List<Room> {room},
                RoomTypes = new List<RoomType> {roomType},
                ContactInfos = new List<ContactInfo>
                {
                    new ContactInfo
                    {
                        Address = new Address
                        {
                            Lines = new List<AddressLine>
                            {
                                new AddressLine {Content = "1 Line Street", LineNo = 1},
                                new AddressLine {Content = "Postal Town 2", LineNo = 2}
                            },
                            CityName = "City Name 1",
                            CountryName = "Country Name 1",
                            PostalCode = "Post code 1"
                        },
                        Name = "Contact McContactFace",
                        PhoneInfos = new List<PhoneInfo> {new PhoneInfo {Number = "+44123476599", Type = "Mobile"}},
                        Type = "Head Office",
                        Email = "headoffice@tenant.com"
                    },
                    new ContactInfo
                    {
                        Address = new Address
                        {
                            Lines = new List<AddressLine>
                            {
                                new AddressLine {Content = "2 Line Street", LineNo = 1},
                                new AddressLine {Content = "Postal Town 2", LineNo = 2}
                            },
                            CityName = "City Name 2",
                            CountryName = "Country Name 2",
                            PostalCode = "Post code 2"
                        },
                        Name = "A. N. Other",
                        PhoneInfos = new List<PhoneInfo> {new PhoneInfo {Number = "+44123476599", Type = "Fax"}},
                        Type = "Main Address",
                        Email = "hotelsales@tenant.com"
                    }
                }
            };

            context.PropertyVersion.Add(propertyVersion);

            var property = new Property()
            {
                Tenant = "Tenant",
                Current = propertyVersion
            };

            context.Properties.Add(property);
            property.Current = propertyVersion;

            context.SaveChanges();
        }

        private static Translations CreateTranslations(string text)
        {
            return new Translations
            {
                Values = new List<Translations.Pair>
                {
                    new Translations.Pair {LanguageTag = "en", Value = "English: " + text},
                    new Translations.Pair {LanguageTag = "fr", Value = "French: " + text},
                }
            };
        }

        [Fact]
        public async void Test()
        {
            var response = await _testClient.GetAsync("api/properties/current/Tenant");

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            dynamic responseObject = JObject.Parse(responseBody);
            Assert.Equal("/api/properties/current/Tenant", responseObject._links.self.href);
        }
    }
}
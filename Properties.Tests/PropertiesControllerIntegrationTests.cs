using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
                Version = "PropertyVersion",
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
                Current = propertyVersion,
                Id = "PropertyId"
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
        public async void GetCurrentTenantsProperties()
        {
            var response = await _testClient.GetAsync("api/properties/current/Tenant");

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            dynamic responseObject = JObject.Parse(responseBody);
            
            Assert.Equal("/api/properties/current/Tenant", responseObject._links.self.href.ToString());
            
        }
        
        [Fact]
        public async void GetProperty()
        {
            var propertyList = await _testClient.GetAsync("api/properties/current/Tenant");
            var responseBody = await propertyList.Content.ReadAsStringAsync();

            dynamic responseObject = JObject.Parse(responseBody);
            var propertyUrl = responseObject._embedded.properties[0]._links.self.href.ToString();

            var propertyResponse = await _testClient.GetAsync(propertyUrl);
            
            Assert.Equal(HttpStatusCode.Redirect, propertyResponse.StatusCode);
            
            Assert.Equal("/api/properties/versions/Tenant/PropertyVersion", propertyResponse.Headers.Location.ToString());
        }
        
        [Fact]
        public async void GetPropertyDirect()
        {
            var propertyList = await _testClient.GetAsync("api/properties/current/Tenant");
            var responseBody = await propertyList.Content.ReadAsStringAsync();

            dynamic responseObject = JObject.Parse(responseBody);
            var propertyUrl = responseObject._embedded.properties[0]._links.direct.href.ToString();

            var propertyResponse = await _testClient.GetAsync(propertyUrl);
            
            Assert.Equal(HttpStatusCode.OK, propertyResponse.StatusCode);
            
            var propertyResponseBody = await propertyResponse.Content.ReadAsStringAsync();
            
            dynamic property = JObject.Parse(propertyResponseBody);
            
            Assert.Equal("English: name", property.name.en.ToString());
        }
        
                
        [Fact]
        public async void CreateProperty()
        {
            var payload = new
            {
                Name = new
                {
                    en = "Name"
                }
            };
            var request = HttpClientHelper.CreateJsonRequest("/api/properties/", HttpMethod.Post, payload);
            var response = await _testClient.SendAsync(request);
            var newLocation = response.Headers.Location;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            Assert.Matches("/api/properties/.*", newLocation.ToString());
            var propertyList = await _testClient.GetAsync("api/properties/current/Tenant");
            
            Assert.Equal(HttpStatusCode.OK, propertyList.StatusCode);
            
            var newProperty = await _testClient.GetAsync(newLocation);
            Assert.Equal(HttpStatusCode.Found, newProperty.StatusCode);
            
            var newPropertyResponse = await _testClient.GetAsync(newProperty.Headers.Location);
            
            Assert.Equal(HttpStatusCode.OK, newPropertyResponse.StatusCode);
            var newPropertyResponseBody = await newPropertyResponse.Content.ReadAsStringAsync();
            
            dynamic responseObject = JObject.Parse(newPropertyResponseBody);
            
            Assert.Equal("Name", responseObject.name.en.ToString());
        }
        
        [Fact]
        public async void UpdateProperty()
        {
            var payload = new
            {
                Name = new
                {
                    en = "New Name"
                }
            };
            var request = HttpClientHelper.CreateJsonRequest("api/properties/current/Tenant/PropertyId", HttpMethod.Put, payload);
            var response = await _testClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            

            var propertyList = await _testClient.GetAsync("api/properties/current/Tenant");
            
            Assert.Equal(HttpStatusCode.OK, propertyList.StatusCode);
            var propertyListBody = await propertyList.Content.ReadAsStringAsync();
            
            dynamic responseObject = JObject.Parse(propertyListBody);

            var link = responseObject._embedded.properties[0]._links.direct.href.ToString();
            Assert.NotEqual("/api/properties/versions/Tenant/PropertyVersion", link);
        }
        
        
        [Fact]
        public async void Delete()
        {
            var request = HttpClientHelper.CreateJsonRequest("api/properties/current/Tenant/PropertyId", HttpMethod.Delete, new {});
            var response = await _testClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            

            var propertyList = await _testClient.GetAsync("api/properties/current/Tenant");
            
            Assert.Equal(HttpStatusCode.NotFound, propertyList.StatusCode);
        }
    }
}
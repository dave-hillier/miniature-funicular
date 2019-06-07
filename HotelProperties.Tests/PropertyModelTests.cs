using System.Collections.Generic;
using Newtonsoft.Json;
using Properties.Model;
using Xunit;

namespace Properties.Tests
{
    public class PropertyVersionTests
    {
        [Fact]
        public void SerializeVersion()
        {
            var roomType = new RoomType
            {
                Id = "RoomTypeId",
                Name = CreateTranslations(),
                Description = CreateTranslations(),
                Images = new List<ImageLink1>(),
                SubRooms = new List<RoomType>(),
                Amenities = new List<OtaAmenity>(),
                Tags = new List<RoomTag>()                
            };
            var room = new Room
            {
                Id = 123,
                Name = CreateTranslations(),
                Description = CreateTranslations(),
                RoomType = roomType,
            };
            var propertyVersion = new PropertyVersion
            {
                Name = CreateTranslations(),
                Description = CreateTranslations(),
                Images = new List<ImageLink>
                {
                    new ImageLink { Href = "http://image/image.png" }                        
                },
                Category = "Category",
                Version = "VersionNo",
                Tenant = "Tenant",
                ContactInfos = new List<ContactInfo> { new ContactInfo
                {
                    Email = "email@what.com",
                    Name = "Name name",
                }},
                Rooms = new List<Room> { room },
                RoomTypes = new List<RoomType> { roomType }
            };

            var json = JsonConvert.SerializeObject(propertyVersion);
            
            Assert.Equal("{\"Name\":{\"en\":\"Value\"},\"Description\":{\"en\":\"Value\"},\"Updated\":\"0001-01-01T00:00:00\",\"Category\":\"Category\",\"ContactInfos\":[{\"Type\":null,\"Name\":\"Name name\",\"Email\":\"email@what.com\",\"Address\":null,\"PhoneInfos\":null}],\"_links\":{\"self\":{\"Href\":\"/api/properties/Tenant/VersionNo\",\"Method\":null},\"images\":[{\"Href\":\"http://image/image.png\",\"Method\":null}]},\"_embedded\":{\"rooms\":[{\"Name\":{\"en\":\"Value\"},\"Description\":{\"en\":\"Value\"},\"_links\":{\"self\":{\"Href\":\"/api/rooms/123\",\"Method\":null},\"roomType\":{\"Href\":\"/api/roomTypes/\",\"Method\":null}},\"_embedded\":null}],\"roomsTypes\":[{\"Name\":{\"en\":\"Value\"},\"Description\":{\"en\":\"Value\"},\"Amenities\":[],\"Tags\":[],\"_links\":{\"self\":{\"Href\":\"/api/roomTypes/RoomTypeId\",\"Method\":null}},\"_embedded\":null}]}}", json);
        }
        
        // TODO: empty contactinfos

        private static Translations CreateTranslations()
        {
            return new Translations
            {
                Values = new List<Translations.Pair>
                {
                    new Translations.Pair {LanguageTag = "en", Value = "Value"}
                }
            };
        }
    }
}
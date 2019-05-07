using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Properties.Model;
using Xunit;

namespace Properties.Tests
{
    public class RoomTypeTests
    {
        [Fact]
        public void Serialize()
        {
            var roomType = new RoomType
            {
                Id = "RoomTypeId",
                Name = CreateTranslations(),
                Description = CreateTranslations(),
                Images = new List<ImageLink1> { new ImageLink1 { Href = "http://localhost/1.jpg"}},
                SubRooms = new List<RoomType> {  },
                Amenities = new List<OtaAmenity> { new OtaAmenity
                {
                    Id = 1, 
                    Value = 2
                }},
                Tags = new List<RoomTag> { new RoomTag { Tag = "Tag" }}              
            };
            var json = JsonConvert.SerializeObject(roomType, 
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            
            Assert.Equal("{\"Name\":{\"en\":\"Value\"},\"Description\":{\"en\":\"Value\"},\"Amenities\":[{\"Id\":1,\"Value\":2}],\"Tags\":[\"Tag\"],\"_links\":{\"self\":{\"Href\":\"/api/roomTypes/RoomTypeId\"},\"images\":[{\"Href\":\"http://localhost/1.jpg\"}]}}", json);
        }
        
        [Fact]
        public void SerializeSubRoom()
        {
            var subRoomType = new RoomType
            {
                Id = "SubRoomTypeId",            
            };
            var roomType = new RoomType
            {
                Id = "RoomTypeId",
                SubRooms = new List<RoomType> { subRoomType }
            };
            var json = JsonConvert.SerializeObject(roomType, 
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            
            Assert.Equal("{\"_links\":{\"self\":{\"Href\":\"/api/roomTypes/RoomTypeId\"}},\"_embedded\":{\"subRooms\":[{\"_links\":{\"self\":{\"Href\":\"/api/roomTypes/SubRoomTypeId\"}}}]}}", json);
        }
        
        [Fact]
        public void Deserialize()
        {
            var json =
                "{ \"Name\": { \"en\": \"Value\" }, \"Description\": { \"en\": \"Value\" }, \"Amenities\": [ {\"Id\": 1, \"Value\": 2 } ], \"Tags\": [ \"Tag\" ], \"Images\": [ \"http://localhost/1.jpg\" ] }";
            var roomType = JsonConvert.DeserializeObject<RoomType>(json);
            
            Assert.Equal("en", roomType.Description.Values.First().LanguageTag);
            Assert.Equal("Value", roomType.Description.Values.First().Value);
            
            Assert.Equal("en", roomType.Name.Values.First().LanguageTag);
            Assert.Equal("Value", roomType.Name.Values.First().Value);
            
            Assert.Equal(1, roomType.Amenities.First().Id);
            Assert.Equal(2, roomType.Amenities.First().Value);

            Assert.Equal("Tag", roomType.Tags.First().Tag);
            Assert.Equal("http://localhost/1.jpg", roomType.Images.First().Href);
        }

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
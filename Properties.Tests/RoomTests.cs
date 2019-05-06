using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Properties.Model;
using Xunit;

namespace Properties.Tests
{
    public class RoomTests
    {
        [Fact]
        public void Serialize()
        {
            var room = new Room
            {
                Id = 99,
                Name = CreateTranslations(),
                Description = CreateTranslations(),        
                // TODO: type
                RoomTypeId = "RoomTypeId",
                RoomType = new RoomType
                {
                    Id = "Wat"
                }
                
            };
            var json = JsonConvert.SerializeObject(room, 
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            
            Assert.Equal("{\"Name\":{\"en\":\"Value\"},\"Description\":{\"en\":\"Value\"},\"_links\":{\"self\":{\"Href\":\"/api/rooms/99\"},\"roomType\":{\"Href\":\"/api/roomTypes/RoomTypeId\"}}}", json);
        }

        [Fact]
        public void Deserialize()
        {
            var json = "{\"Name\":{\"en\":\"Value\"},\"Description\":{\"en\":\"Value\"}}";
            
            var room = JsonConvert.DeserializeObject<Room>(json);
            
            Assert.Equal("en", room.Description.Values.First().LanguageTag);
            Assert.Equal("Value", room.Description.Values.First().Value);
            
            Assert.Equal("en", room.Name.Values.First().LanguageTag);
            Assert.Equal("Value", room.Name.Values.First().Value);
            
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
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Properties.Controllers;
using Properties.Model;

namespace Properties.Converters
{
    public class RoomJsonConverter : JsonConverter
    {
        private class Dto
        {
            public Translations Name { get; set; }
        
            public Translations Description { get; set; }
            
            // TODO: RoomType
            
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var roomType = ((Room) value).ToResource();
            var temp = JObject.FromObject(roomType, serializer);

            temp.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = serializer.Deserialize<Dto>(reader);
            return new Room
            {
                Name = temp.Name,
                Description = temp.Description,
                // TODO: roomtype
            };
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Room);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Properties.Controllers;
using Properties.Model;

namespace Properties.Converters
{
    public class RoomTypeJsonConverter : JsonConverter
    {
        private class Dto
        {
            public Translations Name { get; set; }
        
            public Translations Description { get; set; }
            
            public List<string> Tags { get; set; } = new List<string>();
            
            public List<Dto> SubRooms { get; set; } = new List<Dto>();
            
            public List<string> Images { get; set; } = new List<string>();
            
            public List<OtaAmenity> Amenities { get; set; } = new List<OtaAmenity>();
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var roomType = ((RoomType) value).ToResource();
            var temp = JObject.FromObject(roomType, serializer);

            temp.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = serializer.Deserialize<Dto>(reader);
            return CreateRoomType(temp);
        }

        private RoomType CreateRoomType(Dto temp)
        {
            return new RoomType
                        {
                            Name = temp.Name,
                            Description = temp.Description,
                            Images = temp.Images.Count > 0 ? temp.Images.Select((image, i) => new ImageLink1 { Href = image, SortValue = i}).ToList() : null,
                            Amenities = temp.Amenities,
                            Tags = temp.Tags.Count > 0 ? temp.Tags.Select(t => new RoomTag { Tag = t}).ToList() : null,
                            SubRooms = temp.SubRooms.Select(s => CreateRoomType(temp)).ToList()
                        };
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RoomType);
        }
    }
}
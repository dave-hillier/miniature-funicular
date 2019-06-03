using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Properties.Controllers;
using Properties.Model;

namespace Properties.Converters
{
    public class PropertyVersionJsonConverter : JsonConverter
    {
        private class InnerDto
        {
            public Translations Name { get; set; }

            public Translations Description { get; set; }

            public string Category { get; set; }

            public List<string> Images { get; set; }

            public List<ContactInfo> ContactInfos { get; set; }

            public List<Room> Rooms { get; set; }

            public List<RoomType> RoomTypes { get; set; }

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propertyVersion = ((PropertyVersion)value);
            var temp = JObject.FromObject(propertyVersion.ToResource());
            temp.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = serializer.Deserialize<InnerDto>(reader);
            return new PropertyVersion
            {
                Name = temp.Name,
                Description = temp.Description,
                Category = temp.Category,
                ContactInfos = temp.ContactInfos,
                Images = temp.Images?.Select((image, i) => new ImageLink { Href = image, SortValue = i }).ToList(),
                Rooms = temp.Rooms,
                RoomTypes = temp.RoomTypes
            };
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyVersion);
        }
    }
}
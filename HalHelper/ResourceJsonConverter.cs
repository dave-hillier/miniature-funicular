using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HalHelper
{
    public class ResourceJsonConverter : JsonConverter
    {
        private class Inner
        {
            [JsonProperty(PropertyName = "_links")]
            public Dictionary<string, object> Links { get; set; }
    
            [JsonProperty(PropertyName = "_embedded")]
            public Dictionary<string, List<Resource>> Embedded { get; set; }
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resource = ((Resource) value);
            
            var inner = JObject.FromObject(new Inner
            {
                Links = resource.Links.Count > 0 ? resource.Links : null,
                Embedded = resource.Embedded.Count > 0 ? resource.Embedded : null
            }, serializer);
            
            var state = resource.State;
            if (state != null)
            {
                var obj2 = JObject.FromObject(state, serializer);
                if (obj2 != null)
                {
                    obj2.Merge(inner);
                    inner = obj2;
                }                
            }                       
            inner.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Resource);
        }
    }
}
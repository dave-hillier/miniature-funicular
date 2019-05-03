using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Properties.Model
{    
    public class TranslationsJsonConverter : JsonConverter
    {        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var translation = (Translations) value;           
            var @object = JObject.FromObject(translation.Values.ToDictionary(kv => kv.LanguageTag, kv => kv.Value), serializer);                 
            @object.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Translations);
        }
    }

    
    [JsonConverter(typeof(TranslationsJsonConverter))]
    public class Translations
    {
        [JsonIgnore]
        public int Id { get; set; }

        public List<Pair> Values { get; set; }

        [Owned]
        public class Pair
        {
            public string LanguageTag { get; set; }
        
            public string Value { get; set; } 
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Properties.Model;

namespace Properties.Converters
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
            var dict = serializer.Deserialize<Dictionary<string, string>>(reader);
            return new Translations
            {
                Values = dict.Select(kv => new Translations.Pair { LanguageTag = kv.Key, Value = kv.Value }).ToList()
            };
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Translations);
        }
    }
}
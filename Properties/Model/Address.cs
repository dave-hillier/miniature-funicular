using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Properties.Model
{
    [Owned]
    [JsonConverter(typeof(AddressJsonConverter))]
    public class Address
    {
        public List<AddressLine> Lines { get; set; }
        
        public string CityName { get; set; }

        public string PostalCode { get; set; }
        
        public string CountryName { get; set; }
    }
    
    public class AddressJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var address = ((Address) value);

            var lines = address.Lines.Select(l => l.Content);
            var temp = JObject.FromObject(new 
            {
                Lines = lines,
                address.CityName,
                address.PostalCode,
                address.CountryName
            }, serializer);
 
            temp.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Address);
        }
    }

}
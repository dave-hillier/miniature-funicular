using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Properties.Model;

namespace Properties.Converters
{
    public class AddressJsonConverter : JsonConverter
    {
        private class InnerDto
        {
            public IEnumerable<string> Lines { get; set; }
            public string CityName { get; set;}
            public string PostalCode { get; set;}
            public string CountryName { get; set;}

        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var address = ((Address) value);

            var lines = address.Lines.Select(l => l.Content);
            var temp = JObject.FromObject(new InnerDto
            {
                Lines = lines,
                CityName = address.CityName,
                PostalCode = address.PostalCode,
                CountryName = address.CountryName
            }, serializer);
 
            temp.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = serializer.Deserialize<InnerDto>(reader);
            return new Address
            {
                CityName = temp.CityName,
                CountryName = temp.CountryName,
                Lines = temp.Lines.Select((l, i) => new AddressLine {Content = l, LineNo = i}).ToList(),
                PostalCode = temp.PostalCode
            };
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Address);
        }
    }
}
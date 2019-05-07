using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Properties.Converters;

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
}
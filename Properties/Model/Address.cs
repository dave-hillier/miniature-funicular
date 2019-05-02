using System.Collections.Generic;
using Newtonsoft.Json;

namespace Properties.Model
{
    class Address
    {
        [JsonIgnore]
        public int Id { get; set; }

        public List<AddressLine> Lines { get; set; }
        
        public string CityName { get; set; }

        public string PostalCode { get; set; }
        
        public string CountryName { get; set; }
    }
}
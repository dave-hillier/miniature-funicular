using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        
        [Required]
        public string CityName { get; set; }

        [Required]
        public string PostalCode { get; set; }
        
        [Required]
        public string CountryName { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Properties.Model
{
    public class Property
    {
        [JsonIgnore]
        public string Id { get; set; }

        [Required]
        public PropertyVersion Current { get; set; }

        [JsonIgnore]
        [Required]
        [MaxLength(100)]
        public string Tenant { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Properties.Model
{
    public class Property
    {
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public PropertyVersion Current { get; set; }

        [JsonIgnore]
        [Required]
        [MaxLength(64)]
        public string Tenant { get; set; }
    }
}
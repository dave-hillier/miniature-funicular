using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Properties.Model
{
    public class OtaAmenity 
    {
        [JsonIgnore]
        [Key]
        public RoomType Parent { get; set; }
        
        [Key]
        public int Id { get; set; } // Matches the OTA value
        
        public int? Value { get; set; }       
    }
}
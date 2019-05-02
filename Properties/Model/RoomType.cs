using System.Collections.Generic;
using Newtonsoft.Json;

namespace Properties.Model
{
    
    class RoomType
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        public LocalizableContent Name { get; set; }
        
        public List<RoomTag> Tags { get; set; }
        
        public List<OtaAmenity> Amenities { get; set; }
    }
}
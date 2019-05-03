using System.Collections.Generic;
using Newtonsoft.Json;

namespace Properties.Model
{
    public class RoomType
    {
        public string Id { get; set; }
        
        public Translations Name { get; set; }
        
        public Translations Description { get; set; }
        
        public List<OtaAmenity> Amenities { get; set; }

        public List<RoomTag> Tags { get; set; }
        
        public List<RoomType> SubRooms { get; set; }
        
        public List<ImageLink1> Images { get; set; }
    }
}
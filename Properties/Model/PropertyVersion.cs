using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Properties.Model
{
    public class PropertyVersion
    {
        [JsonIgnore]
        public string Id { get; set; }
        
        [MaxLength(64)]
        [JsonIgnore]
        public string Tenant { get; set; }

        public DateTime Updated { get; set; }
        
        public Translations Name { get; set; }

        public Translations Description { get; set; }
        
        public string Category { get; set; } // TODO: does this need to be a lookup table?

        [JsonIgnore]
        public List<ImageLink> Images { get; set; }
        
        public List<ContactInfo> ContactInfos { get; set; }
        
        [JsonIgnore]
        public List<Room> Rooms { get; set; }
        
        [JsonIgnore]
        public List<RoomType> RoomTypes { get; set; }
        
        // policies
        // check in-out times
        // GPS Coordinates?
    }
}
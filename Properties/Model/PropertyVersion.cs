using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Properties.Model
{
    class PropertyVersion
    {
        [JsonIgnore]
        public string Id { get; set; }
        
        [MaxLength(64)]
        [JsonIgnore]
        public string Tenant { get; set; }

        public DateTime Updated { get; set; }
        
        public LocalizableContent Name { get; set; }

        public LocalizableContent Description { get; set; }
        
        public Address Address { get; set; }
        
        public PhoneInfo PhoneInfo { get; set; }

        public string Category { get; set; }

        public List<PropertyImage> Images { get; set; }

        // TODO: management info, company, address
        public ManagementInfo ManagementInfo { get; set; }
        
        // GPS Coordinates?
    }
}
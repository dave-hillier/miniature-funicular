using System;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Properties.Model
{
    class PropertyThing
    {              
        [JsonIgnore]
        public string Id { get; set; }
        
        public PropertyVersion Current { get; set; }        
        
        [JsonIgnore]
        public string Tenant { get; set; }
    }
}
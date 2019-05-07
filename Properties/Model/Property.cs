using Newtonsoft.Json;

namespace Properties.Model
{
    public class Property
    {              
        [JsonIgnore]
        public string Id { get; set; }
        
        public PropertyVersion Current { get; set; }        
        
        [JsonIgnore]
        public string Tenant { get; set; }
    }
}
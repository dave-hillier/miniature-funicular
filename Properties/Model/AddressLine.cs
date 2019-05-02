using Newtonsoft.Json;

namespace Properties.Model
{
    class AddressLine
    {
        [JsonIgnore]
        public int ParentId { get; set; } // TODO: composite key
        
        [JsonIgnore]
        public int LineNo { get; set; }
        
        public string Content { get; set; }
    }
}
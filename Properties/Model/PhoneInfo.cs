using Newtonsoft.Json;

namespace Properties.Model
{
    class PhoneInfo
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        public string Type { get; set; }
        
        public string Number { get; set; }
    }
}
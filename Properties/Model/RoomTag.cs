using Newtonsoft.Json;

namespace Properties.Model
{
    class RoomTag
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        public string Tag { get; set; }       
    }
}
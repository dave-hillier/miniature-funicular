using Newtonsoft.Json;

namespace Properties.Model
{
    class RoomImage
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        [JsonIgnore]
        public RoomType Parent { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
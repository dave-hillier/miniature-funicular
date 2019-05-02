using Newtonsoft.Json;

namespace Properties.Model
{
    class Room
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        public LocalizableContent Name { get; set; }
        
        public RoomType RoomType { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Properties.Converters;

namespace Properties.Model
{
    [JsonConverter(typeof(RoomJsonConverter))]
    public class Room
    {
        [JsonIgnore]
        public int Id { get; set; }


        //[Required] // Removed because of cascade

        public Translations Name { get; set; }

        public Translations Description { get; set; }

        [JsonIgnore]
        [Required]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        [JsonIgnore]
        public RoomType RoomType { get; set; }
    }
}
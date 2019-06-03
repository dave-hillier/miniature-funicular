using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Properties.Converters;

namespace Properties.Model
{
    [JsonConverter(typeof(RoomTypeJsonConverter))]
    public class RoomType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public Translations Name { get; set; }

        public Translations Description { get; set; }

        public List<OtaAmenity> Amenities { get; set; }

        public List<RoomTag> Tags { get; set; }

        public List<RoomType> SubRooms { get; set; }

        public List<ImageLink1> Images { get; set; }
    }
}
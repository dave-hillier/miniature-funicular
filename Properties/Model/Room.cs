using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Properties.Model
{
    public class Room
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        [Required]
        public Translations Name { get; set; }
        
        [Required]
        public Translations Description { get; set; }
        
        [JsonIgnore]
        public string RoomTypeId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("RoomTypeId")]
        public RoomType RoomType { get; set; }
    }
}
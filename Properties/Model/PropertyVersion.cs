using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Properties.Converters;

namespace Properties.Model
{
    [JsonConverter(typeof(PropertyVersionJsonConverter))]
    public class PropertyVersion
    {
        [Key]
        public string Version { get; set; }

        [Required]
        [MaxLength(100)]
        public string Tenant { get; set; }

        public DateTime Updated { get; set; }

        public Translations Name { get; set; }

        public Translations Description { get; set; }

        [Required]
        public string Category { get; set; } // TODO: does this need to be a lookup table?

        public List<ImageLink> Images { get; set; }

        public List<ContactInfo> ContactInfos { get; set; }

        public List<Room> Rooms { get; set; }

        public List<RoomType> RoomTypes { get; set; }

        // policies
        // check in-out times
        // GPS Coordinates?
    }
}
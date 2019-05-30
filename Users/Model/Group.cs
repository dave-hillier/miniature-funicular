using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Users.Model
{
    public class Group
    {
        [JsonIgnore]
        public string Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }

        // Not on end user model - usually group, sometimes site
        [JsonIgnore]
        [MaxLength(64)]
        [Required]
        public string Tenant { get; set; }


        [MinLength(3)]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public List<UserGroup> UserGroups { get; set; }

    }
}
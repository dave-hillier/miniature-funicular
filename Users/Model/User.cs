using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Users.Model
{
    public class User
    {
        // Not on end user model
        [JsonIgnore]
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }

        // Not on end user model - usually equivalent to group, sometimes site
        [JsonIgnore]
        [MaxLength(64)]
        [Required]
        public string Tenant { get; set; }

        [JsonIgnore]
        public List<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Issues.Model
{
    public class Issue
    {
        [JsonIgnore]
        public string Id { get; set; }

        [MaxLength(64)]
        [JsonIgnore]
        [Required]
        public string Tenant { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime? Resolved { get; set; }

        [MaxLength(2000)]
        [Required]
        public string Location { get; set; } // URL to ...?

        [Required]
        public string Status { get; set; } // TODO: enum
        public List<IssueImage> Images { get; set; }

    }
}
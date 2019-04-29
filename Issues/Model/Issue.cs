using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Issues.Model
{
    public class Issue
    {
        [JsonIgnore]
        public string Id { get; set; }

        [MaxLength(20)]
        [JsonIgnore]
        public string Tenant { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime? Resolved { get; set; }

        public string Location { get; set; } // TODO: locations
        
        public string Status { get; set; } 

        public List<IssueImage> Images { get; set; }
        
        // TODO: action - deduct from stock etc
    }
}
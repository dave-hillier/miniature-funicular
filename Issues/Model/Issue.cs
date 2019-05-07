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
        public string Tenant { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime? Resolved { get; set; }

        public string Location { get; set; } // URL to ...?
        
        public string Status { get; set; } 

        public List<IssueImage> Images { get; set; }
               
    }
}
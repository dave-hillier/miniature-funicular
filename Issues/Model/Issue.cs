using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Issues.Model
{
    public class Issue
    {
        public string Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }

        // Not on end user model - usually group, sometimes site
        [Required]
        [MaxLength(20)]
        public string Tenant { get; set; }
        
        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime? Resolved { get; set; }

        public string Location { get; set; } // TODO: locations
        
        public string Status { get; set; }
        
        // TODO: images
        // TODO: assignee
        // TODO: action - deduct from stock etc
    }
}
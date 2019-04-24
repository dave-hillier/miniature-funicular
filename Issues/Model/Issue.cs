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
    }
}
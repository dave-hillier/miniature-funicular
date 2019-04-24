using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Model
{
    public class User
    {
        // Not on end user model
        public string Id { get; set; }
     
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
        
        // Not on end user model - usually equivalent to group, sometimes site
        [Required]
        [MaxLength(20)]
        public string Tenant { get; set; } 
    }
}
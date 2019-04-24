using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Model
{
    public class Group
    {
        public string Id { get; set; }
        
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string DisplayName { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
    }
}
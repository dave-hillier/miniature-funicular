using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Model
{
    public class User
    {
        public string Id { get; set; }
     
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdated { get; set; }
    }
}
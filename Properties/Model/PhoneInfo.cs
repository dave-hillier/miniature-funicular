using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    [Owned]
    public class PhoneInfo
    {   
        [Required]    
        public string Type { get; set; }
        
        [Required]
        public string Number { get; set; }
    }
}
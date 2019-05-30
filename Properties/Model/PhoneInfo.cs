using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    [Owned]
    public class PhoneInfo
    {
        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Number { get; set; }
    }
}
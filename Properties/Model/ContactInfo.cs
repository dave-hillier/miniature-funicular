using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    [Owned]
    public class ContactInfo
    {
        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public Address Address { get; set; }

        public List<PhoneInfo> PhoneInfos { get; set; }
    }
}
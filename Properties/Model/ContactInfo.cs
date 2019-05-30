using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    [Owned]
    public class ContactInfo
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Address Address { get; set; }

        public List<PhoneInfo> PhoneInfos { get; set; }
    }
}
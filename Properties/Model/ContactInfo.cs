using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    [Owned]
    public class ContactInfo
    {
        public string Type { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public Address Address { get; set; }
        
        public List<PhoneInfo> PhoneInfos { get; set; }  
    }
}
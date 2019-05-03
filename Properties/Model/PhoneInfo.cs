using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Properties.Model
{
    [Owned]
    public class PhoneInfo
    {       
        public string Type { get; set; }
        
        public string Number { get; set; }
    }
}
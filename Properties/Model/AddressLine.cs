using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Properties.Model
{
    [Owned]
    public class AddressLine
    {
        [JsonIgnore]
        public int ParentId { get; set; } 
        
        [JsonIgnore]
        public int LineNo { get; set; }
        
        public string Content { get; set; }
    }
}
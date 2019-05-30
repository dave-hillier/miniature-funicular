using System.ComponentModel.DataAnnotations;
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
        [Required]
        public int LineNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Content { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Properties.Model
{
    [Owned]
    public class RoomTag
    {              
        [Required]
        public string Tag { get; set; }       
    }
}
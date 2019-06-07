using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Properties.Model
{
    [Owned]
    public class ImageLink
    {
        [Required]
        public int SortValue { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Href { get; set; }
    }

    [Owned]
    public class ImageLink1
    {
        [Required]
        public int SortValue { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Href { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Issues.Model
{
    public class IssueImage
    {
        public int Id { get; set; }

        [Required]
        public Issue Parent { get; set; }

        [Required]
        [MaxLength(2000)]

        public string ImageUrl { get; set; }
    }

    public class IssueLocation
    {
        public int Id { get; set; }
        public Issue Parent { get; set; }
        public string Location { get; set; }
    }
}
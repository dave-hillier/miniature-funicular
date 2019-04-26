namespace Issues.Model
{
    public class IssueImage
    {
        public int Id { get; set; }
        public Issue Parent { get; set; }
        public string ImageUrl { get; set; }
    }
}
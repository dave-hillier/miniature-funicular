using System;
using HalHelper;

namespace Issues.Resources
{

    public class IssueResource : ResourceBase
    {
        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime? Resolved { get; set; }

        public string Location { get; set; } // URL
        
        public string Status { get; set; }
        
        // TODO: location, assignee, images
 
        public IssueResource(string selfLink) : base(selfLink)
        {
        }
    }
}
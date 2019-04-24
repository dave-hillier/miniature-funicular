using System;

namespace Issues.Resources
{

    public class PhotoResource : HalResourceBase
    {

    }

    public class AssigneeResource : HalResourceBase
    {

    }



    public class IssueResource : HalResourceBase
    {
        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime? ResolvedAt { get; set; }

        // TODO: location, assignee

    }
}
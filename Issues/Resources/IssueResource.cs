using System;
using HalHelper;

namespace Issues.Resources
{

    public class PhotoResource : HalResourceBase
    {

    }

    public class AssigneeResource : ResourceBase
    {

    }



    public class IssueResource : ResourceBase
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
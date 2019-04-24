using System;
using HalHelper;

namespace Tasks.Resources
{
    public class TaskListResource : ResourceBase
    {
        public TaskListResource(string selfLink) : base(selfLink)
        {
        }

        public string Title { get; set; }
        public DateTime Updated { get; set; }
    }
}
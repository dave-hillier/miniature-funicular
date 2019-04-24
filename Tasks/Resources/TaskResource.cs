using System;
using HalHelper;

namespace Tasks.Resources
{
    public class TaskResource : ResourceBase
    {
        public TaskResource(string selfLink) : base(selfLink)
        {
        }

        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public string Position { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; } // TODO: enum?

        public DateTime? Due { get; set; }

        public DateTime? Completed { get; set; }
    }
}
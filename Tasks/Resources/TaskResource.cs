using System;

namespace Tasks.Resources
{
    public class TaskResource : HalResourceBase
    {
        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public string Position { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; } // TODO: enum?

        public DateTime? Due { get; set; }

        public DateTime? Completed { get; set; }
    }
}
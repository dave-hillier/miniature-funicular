using System;

namespace Tasks.Resources
{


    public class ListResource : HalResourceBase
    {
        public string Title { get; set; }
        public DateTime Updated { get; set; }
    }
}
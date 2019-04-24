using System;
using HalHelper;

namespace Users.Resources
{
    public class UserResource : ResourceBase
    {
        public UserResource(string selfLink) : base(selfLink)
        {
            
        }
        
        public string DisplayName { get; set; }

        public DateTime Updated { get; set; }
    }
}
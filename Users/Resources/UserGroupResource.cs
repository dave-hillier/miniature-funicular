using System;
using HalHelper;

namespace Users.Resources
{
    public class UserGroupResource : ResourceBase
    {
        public UserGroupResource(string selfLink) : base(selfLink)
        {

        }

        public string DisplayName { get; set; }
        
        public DateTime Updated { get; set; }
    }
}
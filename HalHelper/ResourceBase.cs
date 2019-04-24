using System.Collections.Generic;
using Newtonsoft.Json;

namespace HalHelper
{
    public class ResourceBase
    {
        public ResourceBase(string selfLink)
        {
            Links.Add("self", new Link { Href = selfLink });
        }

        [JsonProperty(PropertyName = "_links")]
        public Dictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>();

        [JsonProperty(PropertyName = "_embedded")]
        public Dictionary<string, List<ResourceBase>> Embedded { get; set; } = new Dictionary<string, List<ResourceBase>>();
    }

}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HalHelper
{
    public class ResourceBase
    {
        public ResourceBase(string selfLink)
        {
            Links.Add("self", new LinkResource { Href = selfLink });
        }

        [JsonProperty(PropertyName = "_links")]
        public Dictionary<string, LinkResource> Links { get; set; } = new Dictionary<string, LinkResource>();

        [JsonProperty(PropertyName = "_embedded")]
        public Dictionary<string, List<ResourceBase>> Embedded { get; set; } = new Dictionary<string, List<ResourceBase>>();
    }

}
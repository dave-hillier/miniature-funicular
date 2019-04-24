using System.Collections.Generic;
using Newtonsoft.Json;

namespace Issues.Resources
{
    public class Link
    {
        public string Href { get; set; }

        public string Method { get; set; }

        public string Rel { get; set; }
    }
    public class HalResourceBase
    {
        [JsonProperty(PropertyName = "_links")]
        public Dictionary<string, Link> Links { get; set; }

        [JsonProperty(PropertyName = "_embedded")]
        public Dictionary<string, IEnumerable<HalResourceBase>> Embedded { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HalHelper
{    
    [JsonConverter(typeof(ResourceConverter))]
    public class Resource
    {     
        public object State { get; set; }

        public Resource(string selfLink)
        {
            Links.Add("self", new Link { Href = selfLink });
        }
        
        public Dictionary<string, Link> Links { get; } = new Dictionary<string, Link>();

        public Dictionary<string, List<Resource>> Embedded { get; } = new Dictionary<string, List<Resource>>();       

        public Resource AddLink(string name, string uri)
        {
            Links.Add(name, new Link { Href = uri });
            return this;
        }

        public Resource AddEmbedded(string name, params Resource[] resource)
        {
            return AddEmbedded(name, resource.ToList());
        }

        public Resource AddEmbedded( string name, List<Resource> resource)
        {
            if (Embedded.ContainsKey(name))
                Embedded[name].AddRange(resource);
            else
                Embedded[name] = resource;
            return this;
        }
        
    }
}
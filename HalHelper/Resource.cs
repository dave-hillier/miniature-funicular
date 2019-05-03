using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HalHelper
{    
//    https://tools.ietf.org/html/draft-kelly-json-hal-07

    [JsonConverter(typeof(ResourceJsonConverter))]
    public class Resource
    {     
        public object State { get; set; }

        public Resource(string selfLink)
        {
            Links.Add("self", new Link { Href = selfLink });
        }
        
        public Dictionary<string, object> Links { get; } = new Dictionary<string, object>();

        public Dictionary<string, List<Resource>> Embedded { get; } = new Dictionary<string, List<Resource>>();       

        // Name should follow: https://tools.ietf.org/html/rfc5988
        public Resource AddLink(string name, string uri)
        {            
            Links.Add(name, new Link { Href = uri });
            return this;
        }
        
        public Resource AddLinks(string name, Link[] links)
        {
            if (links.Length == 0)
                return this;
            
            Links.Add(name, links);
            return this;
        }

        public Resource AddEmbedded(string name, params Resource[] resource)
        {           
            return AddEmbedded(name, resource.ToList());
        }

        public Resource AddEmbedded( string name, List<Resource> resource)
        {
            if (resource.Count == 0)
                return this;
            
            if (Embedded.ContainsKey(name))
                Embedded[name].AddRange(resource);
            else
                Embedded[name] = resource;
            return this;
        }
    }
}
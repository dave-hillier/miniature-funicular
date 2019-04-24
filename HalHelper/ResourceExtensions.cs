using System.Collections.Generic;
using System.Linq;

namespace HalHelper
{
    public static class ResourceExtensions
    {
        public static T AddLink<T>(this T resource, string name, string uri) where T : ResourceBase
        {
            resource.Links.Add(name, new Link { Href = uri });
            return resource;
        }

        public static T AddEmbedded<T>(this T parent, string name, params ResourceBase[] resource) where T : ResourceBase
        {
            return AddEmbedded(parent, name, resource.ToList());
        }

        public static T AddEmbedded<T>(this T parent, string name, List<ResourceBase> resource) where T : ResourceBase
        {
            if (parent.Embedded.ContainsKey(name))
                parent.Embedded[name].AddRange(resource);
            else
                parent.Embedded[name] = resource;
            return parent;
        }
    }

}
using Newtonsoft.Json;
using Xunit;

namespace HalHelper.Tests
{
    public class ResourceTest
    {
        [Fact]
        public void CanSerializeEmpty()
        {
            var s = new Resource("/self");

            var result = JsonConvert.SerializeObject(s, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore, 
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

            Assert.Equal("{\"_links\":{\"self\":{\"Href\":\"/self\"}}}", result);
        }
        
        [Fact]
        public void CanSerializeState()
        {
            var s = new Resource("/self") {State = new {Hello = "world"}};

            var result = JsonConvert.SerializeObject(s, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            
            Assert.Equal("{\"Hello\":\"world\",\"_links\":{\"self\":{\"Href\":\"/self\"}}}", result);
        }
        
        [Fact]
        public void CanSerializeEmbedded()
        {
            var s1 = new Resource("/s1");
            var s2 = new Resource("/s2") {State = new { Inside = "here"}};

            s1.AddEmbedded("more", s2);

            var result = JsonConvert.SerializeObject(s1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.Equal("{\"_links\":{\"self\":{\"Href\":\"/s1\"}},\"_embedded\":{\"more\":[{\"Inside\":\"here\",\"_links\":{\"self\":{\"Href\":\"/s2\"}}}]}}", result);
        }
    }
}

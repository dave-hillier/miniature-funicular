using System.Collections.Generic;
using Newtonsoft.Json;
using Properties.Model;
using Xunit;

namespace Properties.Tests
{
    public class TranslationsTests
    {
        [Fact]
        public void Serialize()
        {
            var translation = new Translations
            {
                Values = new List<Translations.Pair>
                {
                    new Translations.Pair {LanguageTag = "en", Value = "English"},
                    new Translations.Pair {LanguageTag = "fr", Value = "French"},
                }
            };

            var json = JsonConvert.SerializeObject(translation);
            
            Assert.Equal("{\"en\":\"English\",\"fr\":\"French\"}", json);
        }

        [Fact]
        public void Deserialize()
        {
            var json = "{\"en\":\"English\",\"fr\":\"French\"}";
            var translation = JsonConvert.DeserializeObject<Translations>(json);
            
            Assert.Equal(2, translation.Values.Count);
            // TODO: check more
        }
    }
}
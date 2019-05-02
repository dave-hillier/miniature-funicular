using Newtonsoft.Json;

namespace Properties.Model
{
    class PropertyImage
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        [JsonIgnore]
        public PropertyVersion Parent { get; set; }
        
        public string Href { get; set; }
    }

    class LocalizableContent
    {
        [JsonIgnore]
        public int Id { get; set; }
     
        public string LanguageTag { get; set; }
        
        public string Value { get; set; }
    }
}
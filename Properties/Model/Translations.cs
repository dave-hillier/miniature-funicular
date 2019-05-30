using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Properties.Converters;

namespace Properties.Model
{
    [JsonConverter(typeof(TranslationsJsonConverter))]
    public class Translations
    {
        [JsonIgnore]
        public int Id { get; set; }

        public List<Pair> Values { get; set; }

        [Owned]
        public class Pair
        {
            [Required]
            public string LanguageTag { get; set; }

            [Required]
            public string Value { get; set; }
        }

    }
}
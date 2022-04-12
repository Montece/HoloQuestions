using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Rating
    {
        [JsonProperty("name")]
        public int Name { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}

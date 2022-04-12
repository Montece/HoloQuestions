using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Genre
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("russian")]
        public string Russian { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }
    }
}

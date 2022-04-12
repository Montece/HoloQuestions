using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Video
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("player_url")]
        public string PlayerUrl { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("hosting")]
        public string Hosting { get; set; }
    }
}

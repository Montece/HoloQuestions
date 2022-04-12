using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub.Structs
{
    [Serializable]
    public class Coub
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("permalink")]
        public string PermaLink { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("channel_id")]
        public int ChannelID { get; set; }
        [JsonProperty("views_count")]
        public int ViewsCount { get; set; }
        [JsonProperty("published_at")]
        public string PublishedAt { get; set; }
        [JsonProperty("picture")]
        public string PictureURL { get; set; }
        [JsonProperty("timeline_picture")]
        public string TimelinePictureURL { get; set; }
        [JsonProperty("small_picture")]
        public string SmallPictureURL { get; set; }
        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }
        [JsonProperty("file_versions")]
        public Versions FileVersion { get; set; }
    }
}
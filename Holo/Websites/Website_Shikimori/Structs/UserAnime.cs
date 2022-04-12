using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class UserAnime
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("anime")]
        public Anime Anime { get; set; }
        [JsonProperty("manga")]
        public object Manga { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("text")]
        public object Text { get; set; }
        [JsonProperty("episodes")]
        public int Episodes { get; set; }
        [JsonProperty("chapters")]
        public int? Chapters { get; set; }
        [JsonProperty("volumes")]
        public int? Volumes { get; set; }
        [JsonProperty("rewatches")]
        public int Rewatches { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}

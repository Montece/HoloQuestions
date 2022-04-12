using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class User
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("image")]
        public UserImage Image { get; set; }
        [JsonProperty("last_online_at")]
        public DateTime LastOnlineAt { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

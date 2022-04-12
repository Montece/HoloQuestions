using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class ProfileInfo
    {
        [JsonProperty("message")]
        public string ErrorMessage { get; set; }
        [JsonProperty("code")]
        public int ErrorID { get; set; } = 0;
    }
}

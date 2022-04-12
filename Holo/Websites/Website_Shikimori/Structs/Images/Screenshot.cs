using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Screenshot
    {
        [JsonProperty("original")]
        public string Original { get; set; }
        [JsonProperty("preview")]
        public string Preview { get; set; }
    }
}

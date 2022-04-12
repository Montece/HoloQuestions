using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class WatchStatus
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}

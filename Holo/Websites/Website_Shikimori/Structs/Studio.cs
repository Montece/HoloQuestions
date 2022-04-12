using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Studio
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("filtered_name")]
        public string FilteredName { get; set; }
        [JsonProperty("real")]
        public bool Real { get; set; }
    }
}

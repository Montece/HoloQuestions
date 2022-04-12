using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub.Structs
{
    [Serializable]
    public class Tag
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}

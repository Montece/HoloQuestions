using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class UserImage
    {
        [JsonProperty("x160")]
        public string X160 { get; set; }
        [JsonProperty("x148")]
        public string X148 { get; set; }
        [JsonProperty("x80")]
        public string X80 { get; set; }
        [JsonProperty("x64")]
        public string X64 { get; set; }
        [JsonProperty("x48")]
        public string X48 { get; set; }
        [JsonProperty("x32")]
        public string X32 { get; set; }
        [JsonProperty("x16")]
        public string X16 { get; set; }
    }
}

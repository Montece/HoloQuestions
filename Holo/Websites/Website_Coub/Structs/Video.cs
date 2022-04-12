using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub.Structs
{
    [Serializable]
    public class Video
    {
        [JsonProperty("default")]
        public string Url { get; set; }
    }
}

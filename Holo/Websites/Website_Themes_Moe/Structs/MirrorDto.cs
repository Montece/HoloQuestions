using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Themes_Moe
{
    [Serializable]
    public class MirrorDto
    {
        [JsonProperty("mirrorURL")]
        public string URL;
        [JsonProperty("priority")]
        public string Priority;
        [JsonProperty("notes")]
        public string Notes;
    }
}

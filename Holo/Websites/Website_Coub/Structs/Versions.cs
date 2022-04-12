using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub.Structs
{
    [Serializable]
    public class Versions
    {
        [JsonProperty("share")]
        public File File { get; set; }
    }
}

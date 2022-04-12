using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub.Structs
{
    [Serializable]
    public class File
    {
        [JsonProperty("default")]
        public string URL { get; set; }
    }
}

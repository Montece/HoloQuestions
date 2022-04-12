using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Coub.Structs
{
    [Serializable]
    public class Container
    {
        [JsonProperty("coubs")]
        public Coub[] Coub { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
    }
}

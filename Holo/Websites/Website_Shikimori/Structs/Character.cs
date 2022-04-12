using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class Character
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("russian")]
        public string Russian { get; set; }
        [JsonProperty("image")]
        public CharacterImage Image { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        //Addition
        [JsonProperty("altname")]
        public string Altername { get; set; }
        [JsonProperty("japanese")]
        public string Japanese { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}

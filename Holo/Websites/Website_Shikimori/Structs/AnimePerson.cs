using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Holo.Websites.Website_Shikimori.Structs
{
    [Serializable]
    public class AnimePerson
    {
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
        [JsonProperty("roles_russian")]
        public List<string> RolesRussian { get; set; }
        [JsonProperty("character")]
        public Character Character { get; set; }
        //Additional
        public Anime Anime { get; set; }
    }
}

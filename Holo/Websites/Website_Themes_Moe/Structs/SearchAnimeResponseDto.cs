using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Holo.Websites.Website_Themes_Moe
{
    [Serializable]
    public class SearchAnimeResponseDto
    {
        [JsonProperty("malID")]
        public int MalID;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("year")]
        public int Year;
        [JsonProperty("season")]
        public string Season;
        [JsonProperty("themes")]
        public List<AnimeMusicDto> Themes;
    }
}
using Newtonsoft.Json;
using System;

namespace Holo.Websites.Website_Themes_Moe
{
    [Serializable]
    public class AnimeMusicDto
    {
        [JsonProperty("themeType")]
        public string ThemeType;
        [JsonProperty("themeName")]
        public string ThemeName;
        [JsonProperty("mirror")]
        public MirrorDto mirror;
    }
}